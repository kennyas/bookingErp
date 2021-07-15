using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.Models;

namespace Report.Core.Services.Interfaces
{
    public interface IBroadcastDashboardService<TBroadcastService, TEntity> where TEntity : AuditedEntity
    {
        TBroadcastService LoadData();
        TBroadcastService LoadData(List<TEntity> dataSource);
        List<TEntity> GetData();
        Task BroadcastAll();
    }
}
