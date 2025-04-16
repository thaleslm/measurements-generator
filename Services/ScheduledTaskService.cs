namespace measurement_generator.Services
{
    public class ScheduledTaskService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

        public ScheduledTaskService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Process(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_interval, stoppingToken);
                await Process(stoppingToken);
            }


        }

        private async Task Process(object stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var cSVReaderService = scope.ServiceProvider.GetRequiredService<CSVReaderService>();

            try
            {
                await cSVReaderService.RegisterCsv();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar: {ex.Message}");

            }
        }
    }
}
