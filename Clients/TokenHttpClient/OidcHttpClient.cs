using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace MtAltinnCommon.Clients.TokenHttpClient;
public class OidcHttpClient
{
    private readonly HttpClient _httpClient;
    private string? _accessToken;
    private string _clientId;
    private string _clientSecret;
    private string _scope;
    private string _keycloakBaseUrl;

    public OidcHttpClient(HttpClient httpClient,String keycloakBaseUrl, String clientId, String clientSecret, String scope)
    {
        if(httpClient == null || keycloakBaseUrl == null || clientId == null || clientSecret == null || scope == null){
            throw new Exception("All constructor parameters must be non-null");
        }
        _httpClient = httpClient;
        _clientId = clientId;
        _clientSecret = clientSecret;
        _scope = scope;
        _keycloakBaseUrl = keycloakBaseUrl;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        if (string.IsNullOrEmpty(_accessToken) || TokenIsExpired())
        {
            var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(_keycloakBaseUrl);
            if (discoveryDocument.IsError)
            {
                throw new Exception($"Discovery document error: {discoveryDocument.Error}");
            }

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = _clientId,
                ClientSecret = _clientSecret
            });

            if (tokenResponse.IsError)
            {
                throw new Exception($"Token error: {tokenResponse.Error}");
            }

            _accessToken = tokenResponse.AccessToken;
        }

        return _accessToken;
    }

    public async Task<HttpResponseMessage> MakeAuthorizedGetRequestAsync(string url)
    {
        var accessToken = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.GetAsync(url);
        return response;
    }

    private bool TokenIsExpired()
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(_accessToken);
        var expirationUnixTime = jwtToken.Payload.Expiration;
        
        if (expirationUnixTime.HasValue)
        {
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expirationUnixTime.Value).UtcDateTime;
            
            return expirationTime < DateTime.UtcNow;
        }

        return true;
    }
}