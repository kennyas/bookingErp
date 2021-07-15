using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;

namespace UserManagement.Core.Services
{
    public class PartnerService : Service<Partner>, IPartnerService
    {
        public PartnerService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
