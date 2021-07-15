using Booking.Core.Utilities.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Core.Context
{
    public class GigBookingContextSeed
    {
        public async Task SeedAsync(GigBookingContext context, IWebHostEnvironment env, IOptions<BookingSettings> settings, ILogger<GigBookingContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(GigBookingContextSeed));
            await policy.ExecuteAsync(async () => {

                var useCustomizationData = settings.Value.UseCustomizationData;
                var contentRootPath = env.ContentRootPath;
                var picturePath = env.WebRootPath;
            });
        }
        private AsyncPolicy CreatePolicy(ILogger<GigBookingContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
