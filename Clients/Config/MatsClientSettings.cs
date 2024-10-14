namespace MtAltinnCommon.Clients.Mats;

public class MatsClientSettings{
    public required string BaseUrl {get;set;}
    public required string KeycloakBaseUrl {get;set;}
    public required string ClientIdSecretName {get;set;}
    public required string ClientSecretSecretName {get;set;}
    public required string Scope {get;set;}
}