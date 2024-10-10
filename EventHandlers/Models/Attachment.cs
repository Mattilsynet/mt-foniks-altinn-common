namespace MtAltinnCommon.Models;

public class Attachment
{
    public string? Path { get; set; }
    public string? ContentType { get; set; }
    public string? FileName { get; set; }
    public long? Size { get; set; }
    public bool? isKvittering { get; set; }
}