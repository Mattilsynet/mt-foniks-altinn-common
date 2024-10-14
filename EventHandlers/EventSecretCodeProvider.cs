using Altinn.App.Core.Internal.Events;
using System.Threading.Tasks;
using System;
using Altinn.App.Core.Internal.Secrets;
using MtAltinnCommon.Config;
using Microsoft.Extensions.Logging;


namespace MtAltinnCommon.EventHandlers;
public class EventSecretCodeProvider : IEventSecretCodeProvider
{
    private string _secretCode = string.Empty;
    private ILogger<EventSecretCodeProvider> _logger;

    /// <inheritdoc/>
    public EventSecretCodeProvider(ISecretsClient secretsClient,MtAltinnSettings settings, ILogger<EventSecretCodeProvider> logger)
    {
        _logger = logger;
        _secretCode = secretsClient.GetSecretAsync(settings.SecretCodeSecretName).Result;
    }

    /// <inheritdoc/>
    public async Task<string> GetSecretCode()
    {
        return await Task.FromResult(_secretCode);
    }
}