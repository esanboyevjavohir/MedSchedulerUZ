using MedSchedulerUZ.Application.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MedSchedulerUZ.Application.Services.Background
{
    public class CertificationExpiryBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public CertificationExpiryBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var certService = scope.ServiceProvider.GetRequiredService<ICertificationService>();
                await certService.CheckExpiringAsync();

                // Har 24 soatda bir marta ishga tushadi
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
