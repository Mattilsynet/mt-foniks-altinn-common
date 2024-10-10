using Altinn.App.Core.Internal.Events;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;


namespace MtAltinnCommon.EventHandlers;
public class EventSecretCodeProvider : IEventSecretCodeProvider
{
    private string _secretCode = string.Empty;
    private ILogger<EventSecretCodeProvider> _logger;

    /// <inheritdoc/>
    public EventSecretCodeProvider(string secretCode, ILogger<EventSecretCodeProvider> logger)
    {
        _logger = logger;
        if (secretCode == null)
        {
            throw new ArgumentException("You must supply a secret code in constructor");
        }
        _secretCode = secretCode;
    }

    /// <inheritdoc/>
    public async Task<string> GetSecretCode()
    {
        return await Task.FromResult(_secretCode);
    }
}