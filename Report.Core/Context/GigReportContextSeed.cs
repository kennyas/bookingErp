using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Report.Core.Utilities.Settings;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Report.Core.Context
{
    public class GigReportContextSeed
    {
        public async Task SeedAsync(GigReportContext context, IWebHostEnvironment env, IOptions<ReportSettings> settings, ILogger<GigReportContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(GigReportContextSeed));
            await policy.ExecuteAsync(() =>
            {

                //var useCustomizationData = settings.Value.UseCustomizationData;
                //var contentRootPath = env.ContentRootPath;
                //var picturePath = env.WebRootPath;

                return null;
            });
        }
        private AsyncPolicy CreatePolicy(ILogger<GigReportContextSeed> logger, string prefix, int retries = 3)
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
