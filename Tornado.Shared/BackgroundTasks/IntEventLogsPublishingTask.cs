using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Wallet.Core.BackGroundJobs
{
    public class IntEventLogsPublishingTask : BackgroundService
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IntEventLogsPublishingTask> _logger;
        private readonly IEventBus _eventBus;

        private static int GracePeriod = 5;
        public IntEventLogsPublishingTask(IConfiguration configuration,
            ILogger<IntEventLogsPublishingTask> logger, IEventBus eventBus)
        {
            _logger = logger;
            _configuration = configuration;
            _eventBus = eventBus;
            connectionString = _configuration.GetConnectionString("Default");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug("#1 GracePeriodManagerService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("GracePeriodManagerService background task is doing background work.");

                 RePublishAndUpdateLatestLogs();

                await Task.Delay(300000, stoppingToken);
            }

            await Task.CompletedTask;
        }

        public void RePublishAndUpdateLatestLogs()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    List<IntegrationEvent> logs = conn.Query<IntegrationEvent>(
                        @"SELECT * FROM IntegrationEventLog 
                            WHERE DATEDIFF(minute, [CreationTime], GETDATE()) >= @GracePeriodTime
                            AND ([State] = 1 or [State] = 3)",
                        new { GracePeriodTime = GracePeriod }).ToList();
                    foreach (var log in logs)
                    {
                        try
                        {
                            _eventBus.Publish(log);
                            conn.Query("Update IntegrationEventLog set [State] = 2 where EventId = @LogId", new { LogId = log.Id });
                        }
                        catch (Exception exception)
                        {
                            _logger.LogCritical(exception, $"FATAL ERROR: Log publishing failed or update could not be saved", exception.Message);
                        }
                    }
                }
                catch (SqlException exception)
                {
                    _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
                }

                catch (Exception exception)
                {
                    _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
                }
            }
        }
    }
}
