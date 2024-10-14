namespace MtAltinnCommon.Clients.Nats;

public class NatsClientSettings{

    public required string JwtSecretName {get; set;}
    public required string SeedSecretName {get; set;}
    public required string Url {get; set;}
    public required int TimeoutMinutes {get;set;}
    public required string ClientName {get;set;}
}