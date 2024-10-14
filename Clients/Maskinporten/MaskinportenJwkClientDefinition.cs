using Altinn.ApiClients.Maskinporten.Config;
using Altinn.ApiClients.Maskinporten.Interfaces;
using Altinn.ApiClients.Maskinporten.Models;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using MtAltinnCommon.Config;
using Altinn.App.Core.Internal.Secrets;

namespace MtAltinnCommon.Clients.Maskinporten;
public class MaskinportenJwkClientDefinition : IClientDefinition
{
    private readonly string _maskinportenJwk;

    public IMaskinportenSettings? ClientSettings { get; set; }

    public MaskinportenJwkClientDefinition(ISecretsClient secretsClient, MtAltinnSettings mtAltinnSettings)
    {
        _maskinportenJwk = secretsClient.GetSecretAsync(mtAltinnSettings.MaskinportenJWKSecretName).Result;
    }
    public async Task<ClientSecrets> GetClientSecrets()
    {
        ClientSecrets clientSecrets = new ClientSecrets();
        string tokenString =_maskinportenJwk;
        byte[] base64EncodedBytes = Convert.FromBase64String(tokenString);
        string jwkjson = Encoding.UTF8.GetString(base64EncodedBytes);
        var keySet = new JsonWebKeySet(jwkjson);
        clientSecrets.ClientKey = keySet.Keys.First();
        return await Task.FromResult(clientSecrets);
    }
}