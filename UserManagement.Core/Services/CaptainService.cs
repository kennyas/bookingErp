using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;

namespace UserManagement.Core.Services
{

    public class CaptainService : Service<Captain> , ICaptainService
    {
        public CaptainService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
