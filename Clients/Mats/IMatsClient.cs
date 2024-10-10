using System.Threading.Tasks;
using MtAltinnCommon.Clients.Mats.Models;

namespace MtAltinnCommon.Clients.Mats;

public interface IMatsClient
{
    public Task<Code?> getCodes(string codeType);

}