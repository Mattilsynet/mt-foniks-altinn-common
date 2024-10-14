namespace MtAltinnCommon.Config;
public class MtAltinnSettings {
    public required string MaskinportenHttpClientName {get;set;}
    public required string MaskinportenJWKSecretName {get; set;}
    public required string NatsTopicName {get;set;}
    public required string SecretCodeSecretName {get;set;}
}