using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.MicroServices.Handlers
{
    public class EmailMessageHandler : IHandleMessages<CartCreatedEvent>
    {
        private readonly ILogger<EmailMessageHandler> _logger;

        public EmailMessageHandler(ILogger<EmailMessageHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(CartCreatedEvent message)
        {
            _logger.LogInformation("Enviando e-mail para {Email} sobre o carrinho {CartId} com valor total de {TotalAmount:C}", 
                message.UserEmail, message.CartId, message.TotalAmount);

            // Simula processamento
            await Task.Delay(1000);

            _logger.LogInformation("E-mail enviado com sucesso para {Email}", message.UserEmail);
        }
    }
}
