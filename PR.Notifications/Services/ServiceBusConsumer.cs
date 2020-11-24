using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace PR.Notifications.Services
{
    public class ServiceBusConsumer
    {
        private readonly ILogger _logger;
        private readonly QueueClient _queueClient;

        public ServiceBusConsumer(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _queueClient = new QueueClient(configuration.GetConnectionString("ServiceBusConnectionString"), "messages");
        }

        public void Register()
        {
            var options = new MessageHandlerOptions((e) => Task.CompletedTask)
            {
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProcessMessage, options);
        }

        private async Task ProcessMessage(Message message, CancellationToken token)
        {
            try
            { 
            var payload = JsonConvert.DeserializeObject<MessagePayload>(Encoding.UTF8.GetString(message.Body));

             _logger.Information("Processing message: " + payload.EventName);

                if (payload.EventName == "NewUserRegistered")
            {
                EmailSender sender = new EmailSender();
                sender.SendNewUserEmail(payload.UserEmail);
            }

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
            catch (Exception e)
            {
                _logger.Error(e, "Something wrong during message process");
                throw;
            }
        }
        private Task ExceptionHandler(ExceptionReceivedEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
   
    public class MessagePayload
    {
        public string EventName { get; set; }
        public string UserEmail { get; set; }
    }
}
