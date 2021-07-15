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
    public interface IWalletService
    {
        Task<ApiResponse<BookingDetailsModel>> BillBooking(BookingEntityModel booking);
    }

    public class WalletService : IWalletService
    {
        private readonly HttpClient _httpClient;
        private readonly WalletServiceUrlsConfig _walletUrls;
        private readonly IHttpUserService _currentUserService;

        static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public WalletService(HttpClient httpClient,
            IOptions<WalletServiceUrlsConfig> walletConfig,
            IHttpUserService currentUserService)
        {
            _httpClient = httpClient;
            _walletUrls = walletConfig.Value;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<BookingDetailsModel>> BillBooking(BookingEntityModel booking)
        {
            var response = new ApiResponse<BookingDetailsModel>(codes: ApiResponseCodes.INVALID_REQUEST);

            if (booking is null)
            {
                response.Errors.Add("Booking not found");
                goto finish;
            }

            await semaphoreSlim.WaitAsync();

            try
            {
                if (booking.Status != BookingStatus.PENDING)
                {
                    response.Errors.Add($"Booking status is :{booking.Status.GetDescription()}");
                    goto finish;
                }

                if (booking.PaymentStatus != PaymentStatus.PENDING)
                {
                    response.Errors.Add($"Booking status is :{booking.Status.GetDescription()}");
                    goto finish;
                }

                //Todo: should pay button be binded to the current user ?
                if (booking.CustomerId != _currentUserService.GetCurrentUser().UserId)
                {
                    response.Errors.Add($"Booking does not belong to the current customer");
                    goto finish;
                }

                var body = new WalletDebitModel
                {
                    CustomerId = booking.CustomerId,
                    Amount = booking.Amount,
                    Reference = booking.RefCode,
                    TransactionDesc = $"Booking payment: #{booking.RefCode} for {booking.CustomerEmail}"
                };

                var walletResponse = await _httpClient.PostAsJsonAsync<WalletDebitModel, WalletDebitModel>
                         (_walletUrls.Base + WalletServiceUrlsConfig.Debit(), body);

                response.Code = walletResponse.Code;
                response.Description = walletResponse.Description;

                if (walletResponse.HasErrors || walletResponse.Code!=ApiResponseCodes.OK)
                    response.Errors.AddRange(walletResponse.Errors);
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