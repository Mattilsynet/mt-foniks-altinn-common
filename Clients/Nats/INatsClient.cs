using System;
using System.Threading.Tasks;

namespace MtAltinnCommon.Clients.Nats;

public interface INatsClient
{
    public Task<bool> Publish(string message, string topic);
    public Task<Guid?> UploadFile(byte[] file);

}