using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.ObjectStore;
using Altinn.App.Core.Internal.Secrets;
using MtAltinnCommon.Config;
namespace MtAltinnCommon.Clients.Nats;

public class NatsClient : INatsClient
{
    private readonly ILogger _logger;

    private readonly string _natsSeed;
    private readonly string _natsJwt;
    private readonly string _natsClientName;
    private readonly string _natsUrl;
    private readonly int _natsTimeoutMinutes;

    public NatsClient(ILogger<NatsClient> logger, NatsClientSettings natsSettings, ISecretsClient secretsClient){
        _logger = logger;
        _natsJwt = secretsClient.GetSecretAsync(natsSettings.JwtSecretName).Result;
        _natsSeed = secretsClient.GetSecretAsync(natsSettings.SeedSecretName).Result;
        _natsClientName = natsSettings.ClientName;
        _natsUrl = natsSettings.Url;
        _natsTimeoutMinutes = natsSettings.TimeoutMinutes;
        if (_natsJwt == null || _natsSeed == null || _natsUrl == null || _natsClientName == null)
        {
            throw new Exception("jwt, seed, natsUrl and nadsClientName must be set");
        }
    }

    public async Task<bool> Publish(string message, string topic)
    {
        try
        {
            return await PublishInternal(message, topic);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not publish message");
            return false;
        }
    }

    private async Task<bool> PublishInternal(string message, string topic)
    {
        try
        {
            NatsOpts opts = new NatsOpts
            {
                AuthOpts = new NatsAuthOpts
                {
                    Jwt = _natsJwt,
                    Seed = _natsSeed
                },
                ConnectTimeout = new TimeSpan(0, _natsTimeoutMinutes, 0),
                Url = _natsUrl,
                Name = _natsClientName
            };

            await using var c = new NatsConnection(opts);
            await c.ConnectAsync();
            var jetstream = new NatsJSContext(c);
            var result = await jetstream.PublishAsync<byte[]>(subject: topic, data: System.Text.Encoding.UTF8.GetBytes(message));
            _logger.LogError("Publiserte melding på: " + topic);

            result.EnsureSuccess();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Feil med publisering " + topic + " Feilen var " + e.Message);
            _logger.LogError(e.StackTrace);
            _logger.LogError(e.ToString());
            return false;
        }
    }

    public async Task<Guid?> UploadFile(byte[] file)
    {
        try
        {
            Guid guid = Guid.NewGuid();
            NatsOpts opts = new NatsOpts
            {
                AuthOpts = new NatsAuthOpts
                {
                    Jwt = _natsJwt,
                    Seed = _natsSeed
                },
                ConnectTimeout = new TimeSpan(0, _natsTimeoutMinutes, 0),
                Url = _natsUrl,
                Name = _natsClientName
            };

            var c = new NatsConnection(opts);
            var jetstream = new NatsJSContext(c);
            var objContext = new NatsObjContext(jetstream);
            var objStore = await objContext.GetObjectStoreAsync("altinn-aktivitet");

            var info = await objStore.PutAsync(guid.ToString(), new MemoryStream(file));

            await c.DisposeAsync();
            return guid;
        }
        catch (Exception e)
        {
            _logger.LogError("Feil med publisering av fil, Feilen var " + e.Message);
            _logger.LogError(e.StackTrace);
            _logger.LogError(e.ToString());
        }

        return null;
    }
}