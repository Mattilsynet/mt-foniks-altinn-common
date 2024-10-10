using Altinn.App.Core.Features;
using Altinn.App.Core.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MtAltinnCommon.EventHandlers;
public class SubscriptionValidationHandler : IEventHandler
{
    private readonly ILogger _logger;
    public SubscriptionValidationHandler(ILogger<SubscriptionValidationHandler> logger)
    {
        _logger = logger;
    }
    public string EventType => "platform.events.validatesubscription";

    public Task<bool> ProcessEvent(CloudEvent cloudEvent)
    {
        _logger.LogError("Received event: " + cloudEvent.Type + ", returning true");
        return Task.FromResult(true);
    }
}