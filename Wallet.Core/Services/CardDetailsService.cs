using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.Dapper.Interfaces;
using Tornado.Shared.Dapper.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.ViewModels;
using Wallet.Core.Models;
using Wallet.Core.Services.Interfaces;
using Wallet.Core.ViewModels;
using System.Linq;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Timing;

namespace Wallet.Core.Services
{
    public class CardDetailsService : Service<CardDetails>, ICardDetailsService
    {
        private readonly IHttpUserService _currentUserService;


        public CardDetailsService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }


        public async Task<ApiResponse<List<CardDetailsViewModel>>> GetCardDetails(GetCardDetailsRequestViewModel model)
        {
            if (model == null)
                return new ApiResponse<List<CardDetailsViewModel>>(codes: ApiResponseCodes.INVALID_REQUEST, message: "invalid request");

            if (!Guid.TryParse(model.CustomerId, out Guid customerId))
                return new ApiResponse<List<CardDetailsViewModel>>(codes: ApiResponseCodes.INVALID_REQUEST, message: "invalid request");
            
            IDictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "customerId", customerId }
            };

            if (!Equals(customerId, _currentUserService.GetCurrentUser().UserId))
            {
                return new ApiResponse<List<CardDetailsViewModel>> (codes: ApiResponseCodes.FAILED, message: "Suspected fraud");
            }
            var cards = await UnitOfWork.Repository<CardDetailsViewModel>().FindAsync("select * from CardDetails where CustomerId = @customerId and IsDeleted = 0", parameter);

            return new ApiResponse<List<CardDetailsViewModel>>(codes: ApiResponseCodes.OK, data: cards.ToList());
        }


        public async Task<ApiResponse<CardDetails>> CreateCardDetails(CreateCardDetailsViewModel model)
        {
            if (model == null)
                return new ApiResponse<CardDetails>(codes: ApiResponseCodes.INVALID_REQUEST, message: "invalid request");

            if (!Guid.TryParse(model.CustomerId, out Guid customerId))
                return new ApiResponse<CardDetails>(codes: ApiResponseCodes.INVALID_REQUEST, message: "invalid request");



            IDictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "customerId", _currentUserService.GetCurrentUser().UserId }
            };

            if (!Equals(customerId, _currentUserService.GetCurrentUser().UserId))
            {
                return new ApiResponse<CardDetails>(codes: ApiResponseCodes.FAILED, message: "Suspected fraud");
            }

            var cards = await UnitOfWork.Repository<CardDetailsViewModel>().FindAsync("select * from CardDetails where CustomerId = @customerId and IsDeleted = 0", parameter);

            if (cards.Count() >= 3)
                return new ApiResponse<CardDetails>(codes: ApiResponseCodes.FAILED, message: "You have exceeded the number of cards that can be added");

            await AddAsync(new CardDetails
            {
                CardType = model.CardType,
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now,
                LastFourDigits = model.LastFourDigits,
                CustomerId = customerId,
                AuthCode = model.AuthCode
            });

            return new ApiResponse<CardDetails>(codes: ApiResponseCodes.OK, message: "Successful");

        }

        public async Task<ApiResponse<bool>> RemoveCards(DeleteCardViewModel model)
        {
            if (model == null)
                return new ApiResponse<bool>(codes: ApiResponseCodes.INVALID_REQUEST, message: "invalid request");

            if (!Guid.TryParse(model.CustomerId, out Guid customerId))
                return new ApiResponse<bool>(codes: ApiResponseCodes.INVALID_REQUEST, message: "invalid request");

            if (!Equals(customerId, _currentUserService.GetCurrentUser().UserId))
            {
                return new ApiResponse<bool>(codes: ApiResponseCodes.FAILED, message: "Suspected fraud");
            }

            var cardDetail = await UnitOfWork.Repository<CardDetails>().GetByIdAsync(model.CardDetailsId);

            if (cardDetail == null || Equals(cardDetail?.CustomerId,  customerId))
            {
                return new ApiResponse<bool>(codes: ApiResponseCodes.NOT_FOUND, message: "Card does not exist");
            }

            await DeleteAsync(cardDetail);

            return new ApiResponse<bool>(codes: ApiResponseCodes.OK, message: "Successful");

        }
    }
}
