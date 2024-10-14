using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Altinn.App.Core.Features;
using Altinn.App.Core.Models;
using Json.More;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MtAltinnCommon.Clients.Nats;
using MtAltinnCommon.Models;
using MtAltinnCommon.Config;

namespace MtAltinnCommon.EventHandlers;

public class InstanceCompleteEventHandler : IEventHandler
{
    private readonly INatsClient _natsClient;
    private readonly ILogger _logger;
    private readonly HttpClient _maskinportenClient;
    private readonly string _natsTopicName;

    public string EventType => "app.instance.process.completed";

    public InstanceCompleteEventHandler(INatsClient natsClient, ILogger<InstanceCompleteEventHandler> logger,
        IHttpClientFactory clientFactory, MtAltinnSettings altinnSettings)
    {
        _natsTopicName = altinnSettings.NatsTopicName;
        _natsClient = natsClient;
        _logger = logger;
        _maskinportenClient = clientFactory.CreateClient(altinnSettings.MaskinportenHttpClientName);
    }

    public async Task<bool> ProcessEvent(CloudEvent cloudEvent)
    {
        try
        {
            CloudEventData? skjemadata = await GetCloudEventData(cloudEvent);
            if (skjemadata != null && skjemadata.data != null)
            {
                var data = await GetNatsAltinnPayload(skjemadata.data);
                if (data != null)
                {
                    return await PublishDataToNats(data);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while processing event");
            return false;
        }

        _logger.LogError("Ferdig å prossesere event, returnerer true");
        return true;
    }
    private async Task<NatsAltinnPayload?> GetNatsAltinnPayload(List<Datum> data)
    {
        var payload = new NatsAltinnPayload();

        foreach (var datum in data)
        {
            if (datum.selfLinks is not null && datum.selfLinks.platform is not null && datum.contentType is not null)
            {
                var responseData = await GetAltinnData(datum.selfLinks.platform, datum.contentType);

                if (datum.filename == null && payload.Data is not null)
                {
                    payload.Data.Add(new DataModel()
                    {
                        Name = datum.dataType,
                        ContentType = datum.contentType,
                        Data = responseData as string
                    });
                }
                else
                {
                    if (responseData is not null && responseData is byte[] byteData)
                    {
                        Guid? guid = await _natsClient.UploadFile(byteData);
                        if (guid != null && payload.Attachments is not null && datum.dataType is not null)
                        {
                            payload.Attachments.Add(new MtAltinnCommon.Models.Attachment()
                            {
                                Path = guid.ToString(),
                                FileName = datum.filename,
                                ContentType = datum.contentType,
                                Size = datum.size,
                                isKvittering = datum.dataType.Equals("ref-data-as-pdf")
                            });
                        }
                        else
                        {
                            _logger.LogError("Klarte ikke laste opp fil");
                        }
                    }
                }
            }
        }

        return payload;
    }

    private async Task<object> GetAltinnData(string dataUrl, string contentType)
    {
        var responseMessage = await _maskinportenClient.GetAsync(dataUrl);

        if (!responseMessage.IsSuccessStatusCode)
        {
            _logger.LogError("Klarte ikke hente skjemadata: " + responseMessage.ReasonPhrase);
            throw new Exception("Klarte ikke å hente skjemadata: " + responseMessage.ReasonPhrase);
        }

        if (contentType == "application/xml")
        {
            return await responseMessage.Content.ReadAsStringAsync();
        }

        return await responseMessage.Content.ReadAsByteArrayAsync();
    }

    private async Task<bool> PublishDataToNats(NatsAltinnPayload eierinseminordata)
    {
        var json = JsonConvert.SerializeObject(eierinseminordata);
        return await _natsClient.Publish(json, _natsTopicName);
    }

    private async Task<CloudEventData?> GetCloudEventData(CloudEvent cloudEvent)
    {
        var baseSkjemaResponse = await _maskinportenClient.GetAsync(cloudEvent.Source.ToString());
        if (!baseSkjemaResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Fikk ikke 200 fra altinn paa cloudEvent.Source: " + baseSkjemaResponse.ReasonPhrase);
            return null;
        }

        var baseSkjemaContent = await baseSkjemaResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<CloudEventData>(baseSkjemaContent);
    }
}