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
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                // Calcular o próximo múltiplo de 15 minutos
                int minutesToNextQuarter = 15 - (now.Minute % 15);
                if (minutesToNextQuarter == 15 && now.Second == 0)
                    minutesToNextQuarter = 0;

                var nextRealRun = new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    0
                ).AddMinutes(minutesToNextQuarter);

                var delay = nextRealRun - now;
                if (delay.TotalMilliseconds > 0)
                {
                    Console.WriteLine($"Aguardando até {nextRealRun:HH:mm:ss} para executar tarefa...");
                    await Task.Delay(delay, stoppingToken);
                }

                // create a "fake Timestamp" with 2023 year
                var simulatedExecutionTime = new DateTime(
                    2023,
                    nextRealRun.Month,
                    nextRealRun.Day,
                    nextRealRun.Hour,
                    nextRealRun.Minute,
                    0
                );

                await Process(stoppingToken,simulatedExecutionTime); // <-- passa só o valor simulado
            }


        }

        private async Task Process(object stoppingToken,DateTime executionTime)
        {
            Console.Write($"execute time: {executionTime}");
            using var scope = _serviceProvider.CreateScope();
            var cSVReaderService = scope.ServiceProvider.GetRequiredService<CSVReaderService>();
            var eRPService = scope.ServiceProvider.GetRequiredService<ErpsService>();

            try
            {
                bool thereAreRegistredErps = await eRPService.ExistErpsInTheDB();
                if(thereAreRegistredErps == false)
                {
                    await cSVReaderService.RegisterCsv();
                    await cSVReaderService.VerifyWhatErpHaveCSV();
                }
                await cSVReaderService.ReadCsvfromErps(executionTime);



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar: {ex.Message}");

            }
        }
    }
}
