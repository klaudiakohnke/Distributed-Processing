using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DP.Patients.KK.Services
{
    public class ServiceBusSender
    {
        private readonly QueueClient _queueClient;

        public ServiceBusSender(IConfiguration configuration)
        {
            _queueClient = new QueueClient(configuration.GetConnectionString("ServiceBusConnectionString"), "messages");
        }

        public async Task SendMessage(MessagePayload payload)
        {
            string data = JsonConvert.SerializeObject(payload);
            Message message = new Message(Encoding.UTF8.GetBytes(data));

            await _queueClient.SendAsync(message);
        }
    }

    public class MessagePayload
    {
        public string EventName { get; set; }
        public string UserEmail { get; set; }
    }
}
