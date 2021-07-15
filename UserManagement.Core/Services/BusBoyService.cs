using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;

namespace UserManagement.Core.Services
{
    public class BusBoyService : Service<BusBoy>, IBusBoyService
    {
        public BusBoyService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}