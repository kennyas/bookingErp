using System.Threading.Tasks;

namespace Report.Core.Services.Interfaces
{
    public interface IBroadcastService
    {
        Task BroadcastToAllConnectedClients();
    }
}
