using Booking.Payment.HttpAggregator.core.Models;
using Booking.Payment.HttpAggregator.core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.ViewModels;

namespace Booking.Payment.HttpAggregator.Controllers
{
    [Route("aggregator/[controller]/[action]")]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly IWalletService _walletService;

        public BookingController(IBookingService bookingService, IWalletService walletService)
        {
            _bookingService = bookingService;
            _walletService = walletService;
        }

        [HttpPost]
        public async Task<ApiResponse<BookingDetailsModel>> Pay(PayModelRequest model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<BookingDetailsModel>(errors: ListModelErrors.ToArray());
               
                var booking = await _bookingService.GetEntityBooking(model.Refcode);
                
                if(model.IsGigPay)
                {
                    booking.GIGRefCode = model.GIGRefCode;
                    booking.Channel = model.Channel;
                    return await _bookingService.ConfirmBookingViaRefcode(booking);
                }

                return await _walletService.BillBooking(booking);

            }).ConfigureAwait(false);
        }
    }
}