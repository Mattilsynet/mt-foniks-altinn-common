using Altinn.App.Core.EFormidling;
using Altinn.App.Core.Internal.Events;
using Altinn.App.Core.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace MtAltinnCommon.EventHandlers;

public class EventSubscriptionService : IHostedService
{
    private readonly AppIdentifier _appIdentifier;
    private readonly IEventsSubscription _eventsSubscriptionClient;
    private readonly ILogger<EventSubscriptionService> _logger;

    public EventSubscriptionService(AppIdentifier appId, IEventsSubscription eventsSubscriptionClient, ILogger<EventSubscriptionService> logger)
    {
        _appIdentifier = appId;
        _eventsSubscriptionClient = eventsSubscriptionClient;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogError("Starting event subscription service");
        var eventType = "app.instance.process.completed";
        try
        {
            var subscription = await _eventsSubscriptionClient.AddSubscription(_appIdentifier.Org, _appIdentifier.App, eventType);
            _logger.LogError("Successfully subscribed to event {eventType} for app {appIdentifier}. Subscription {subscriptionId} is being used.", eventType, _appIdentifier, subscription.Id);
        }

        catch (Exception ex)
        {
            _logger.LogError("Unable to subscribe to event {eventType} for app {appIdentifier}. Received exception {exceptionMessage} with {stackTrace}", eventType, _appIdentifier, ex.Message, ex.StackTrace);
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stop async in event subscription service was called");
        return Task.CompletedTask;
    }

}