using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MtAltinnCommon.Clients.Mats.Models;
using MtAltinnCommon.Clients.TokenHttpClient;

namespace MtAltinnCommon.Clients.Mats;

public class MatsClient : IMatsClient
{
    OidcHttpClient _matsHttpClient;
    private readonly string _matsBaseUrl;
    public MatsClient(string matsBaseUrl, string keycloakBaseUrl, string clientId, string clientSecret, string scope)
    {
        _matsHttpClient = new OidcHttpClient(new HttpClient(), keycloakBaseUrl, clientId, clientSecret, scope);
        _matsBaseUrl = matsBaseUrl;
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