using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.Dapper.Services;
using Tornado.Shared.ViewModels;
using Wallet.Core.Models;
using Wallet.Core.ViewModels;

namespace Wallet.Core.Services.Interfaces
{
    public interface ICardDetailsService : IService<CardDetails>
    {
        Task<ApiResponse<List<CardDetailsViewModel>>> GetCardDetails(GetCardDetailsRequestViewModel model);
        Task<ApiResponse<CardDetails>> CreateCardDetails(CreateCardDetailsViewModel model);
        Task<ApiResponse<bool>> RemoveCards(DeleteCardViewModel model);
    }
}
