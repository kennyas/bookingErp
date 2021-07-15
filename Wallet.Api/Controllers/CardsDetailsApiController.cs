using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.ViewModels;
using Wallet.Core.Models;
using Wallet.Core.Services.Interfaces;
using Wallet.Core.ViewModels;

namespace Wallet.Api.Controllers
{
    [Route("api/[controller]/[action]")]

    public class CardsDetailsApiController : BaseController
    {
        public readonly ICardDetailsService _cardDetailsService;
        public CardsDetailsApiController(ICardDetailsService cardDetailsService)
        {
            _cardDetailsService = cardDetailsService;
        }
        [HttpPost]
          [AllowAnonymous]
        public async Task<ApiResponse<CardDetails>> CreateCardDetail([FromBody] CreateCardDetailsViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                return await _cardDetailsService.CreateCardDetails(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        //  [AllowAnonymous]
        public async Task<ApiResponse<bool>> RemoveCard([FromQuery]DeleteCardViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                return await _cardDetailsService.RemoveCards(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        [HttpGet]
        //  [AllowAnonymous]
        public async Task<ApiResponse<List<CardDetailsViewModel>>> GetCardDetails([FromQuery]GetCardDetailsRequestViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                return await _cardDetailsService.GetCardDetails(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}