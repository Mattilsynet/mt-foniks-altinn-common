using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MtAltinnCommon.Clients.Mats.Models;
using MtAltinnCommon.Clients.TokenHttpClient;
using MtAltinnCommon.Config;
using Altinn.App.Core.Internal.Secrets;

namespace MtAltinnCommon.Clients.Mats;

public class MatsClient : IMatsClient
{
    OidcHttpClient _matsHttpClient;
    private readonly string _matsBaseUrl;
    public MatsClient(MatsClientSettings settings, ISecretsClient secretsClient)
    {
        var clientId = secretsClient.GetSecretAsync(settings.ClientIdSecretName).Result;
        var clientSecret = secretsClient.GetSecretAsync(settings.ClientSecretSecretName).Result;
        _matsHttpClient = new OidcHttpClient(new HttpClient(), settings.KeycloakBaseUrl, clientId , clientSecret, settings.Scope);
        _matsBaseUrl = settings.BaseUrl;
    }
    public async Task<Code?> getCodes(string codeType)
    {
        var baseUrl = _matsBaseUrl;
        var codeUrl = $"/api/v1/codes/{codeType}?inaktive=false";
        var response = await _matsHttpClient.MakeAuthorizedGetRequestAsync(baseUrl+codeUrl);
        var responseString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Code>(responseString);
    }
}