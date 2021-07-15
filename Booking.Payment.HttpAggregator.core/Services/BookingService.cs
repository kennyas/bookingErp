using Booking.Payment.HttpAggregator.core.Helpers;
using Booking.Payment.HttpAggregator.core.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Enums;
using Tornado.Shared.Helpers;
using Tornado.Shared.ViewModels;

namespace Booking.Payment.HttpAggregator.core.Services
{
    public interface IBookingService
    {
        Task<BookingEntityModel> GetEntityBooking(string refcode);
        Task<ApiResponse<BookingDetailsModel>> ConfirmBookingViaRefcode(BookingEntityModel model);
    }

    public class BookingService : IBookingService
    {
        private readonly HttpClient _httpClient;
        private readonly BookingServiceUrlsConfig _bookingUrls;
        static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly IHttpUserService _currentUserService;



        public BookingService(HttpClient httpClient, IOptions<BookingServiceUrlsConfig> bookingConfig,
            IHttpUserService currentUserService)
        {
            _httpClient = httpClient;
            _bookingUrls = bookingConfig.Value;
            _currentUserService = currentUserService;
        }

        public async Task<BookingEntityModel> GetEntityBooking(string refcode)
        {
            var response = await _httpClient.GetAsync<BookingEntityModel>
                     (_bookingUrls.Base + BookingServiceUrlsConfig.GetByRefCode(refcode));

            return response.Payload;
        }

        public async Task<ApiResponse<BookingDetailsModel>> ConfirmBookingViaRefcode(BookingEntityModel model)
        {
            var response = new ApiResponse<BookingDetailsModel>(codes: ApiResponseCodes.INVALID_REQUEST);


            await semaphoreSlim.WaitAsync();

            try
            {

                if (model.Status != BookingStatus.PENDING)
                {
                    response.Errors.Add($"Booking status is :{model.Status.GetDescription()}");
                    goto finish;
                }

                if (model.PaymentStatus != PaymentStatus.PENDING)
                {
                    response.Errors.Add($"Booking status is :{model.Status.GetDescription()}");
                    goto finish;
                }


                if (model.CustomerId != _currentUserService.GetCurrentUser().UserId)
                {
                    response.Errors.Add($"Booking does not belong to the current customer");
                    goto finish;
                }

                var refCodeValidModel = await _httpClient
                                    .PostAsJsonAsync<BookingEntityModel, bool>
                                    (_bookingUrls.Base + BookingServiceUrlsConfig
                                    .VerifyGIGLExternalBookingRefCode, model);


                response.Code = refCodeValidModel.Code;
                response.Description = refCodeValidModel.Description;

                if (refCodeValidModel.HasErrors || refCodeValidModel.Code != ApiResponseCodes.OK)
                    response.Errors.AddRange(refCodeValidModel.Errors);
            }
            finally
            {
                semaphoreSlim.Release();
            }

        finish:
            return response;
        }
    }
}