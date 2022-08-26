using PHVN_WS_CORE.SERVICES.Braze;
using PHVN_WS_CORE.SHARED.Configurations;
using PHVN_WS_CORE.SHARED.Constants;
using PHVN_WS_CORE.SHARED.Extensions;
using PHVN_WS_CORE.Workers;

namespace PHVN_WS_CORE
{
    public sealed class Worker : IHostedService, IAsyncDisposable
    {
        private readonly Task _completedTask = Task.CompletedTask;
        private readonly ILogger<Worker> _logger;
        private IServiceProvider _services { get; }
        protected AppSettings baseAppSettings;

        private Timer? _timer;
        private static int _timerUsing = 0;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceProvider services)
        {
            _logger = logger;
            _services = services ?? throw new ArgumentNullException(nameof(services));

            var _appSettings = configuration.GetOptions<List<AppSettings>>("AppSettings") ?? throw new ArgumentNullException(nameof(AppSettings));

            if (_appSettings == null)
                throw new ArgumentNullException(nameof(AppSettings));

            //Find setting by appId
            baseAppSettings = _appSettings?.FirstOrDefault(x => x.AppId.Equals(Constants.BASE)) ?? throw new ArgumentNullException("baseAppSettings");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{name} is running.", nameof(Worker));

            _timer = new System.Threading.Timer(ServiceProcessing,
                                                null,
                                                TimeSpan.Zero,
                                                baseAppSettings.RollingInvervalConvert);

            return _completedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{name} is stopping.", nameof(Worker));

            _timer?.Change(Timeout.Infinite, 0);

            return _completedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_timer is IAsyncDisposable timer)
            {
                await timer.DisposeAsync();
            }

            _timer = null;
        }

        #region SERVICE PROCESSES
        private async void ServiceProcessing(object? state)
        {
            var startTimeSpan = baseAppSettings?.StartTimestamp ?? DateTime.Now;
            var endTimeSpan = baseAppSettings?.EndTimestamp ?? DateTime.Now;

            if (DateTime.Now.CompareTo(startTimeSpan) >= 0 && DateTime.Now.CompareTo(endTimeSpan) <= 0)
            {
                if (Interlocked.Exchange(ref _timerUsing, 1) == 1)
                {
                    _logger.LogInformation("{name} is processing.", nameof(ServiceProcessing));
                    return;
                }

                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var scopedProcessingService = scope.ServiceProvider.GetRequiredService<GetCheeWorker>();
                        await scopedProcessingService.DoWork();
                    }
                }
                finally
                {
                    Interlocked.Exchange(ref _timerUsing, 0);
                }
            }
        }
        #endregion
    }
}