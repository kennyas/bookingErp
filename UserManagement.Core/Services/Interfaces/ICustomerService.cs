using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Models;
using UserManagement.Core.ViewModels;

namespace UserManagement.Core.Services.Interfaces
{
    public interface ICustomerService : IService<Customer>
    {
        //Task<ApiResponse<CustomerRegisterViewModel>> RegisterAsync(CustomerRegisterViewModel model);
        //Task<ApiResponse<TokenResponseViewModel>> LoginAsync(CredentialsViewModel credentials);
        //Task<ApiResponse<TokenResponseViewModel>> VerifyUser(VerifyUserModel model);
    }
}
