using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace InventoryManagement.Messaging
{
    public class RabbitMqBusService : IHostedService
    {
        private readonly IBusControl _busControl;

        /// <summary>
        /// Initialize analytic bus service
        /// </summary>
        /// <param name="busControl"></param>
        public RabbitMqBusService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _busControl.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}