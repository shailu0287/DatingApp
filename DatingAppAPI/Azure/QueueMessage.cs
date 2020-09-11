using DatingApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;
using Newtonsoft;
using Newtonsoft.Json;
using System.Text;

namespace DatingAppAPI.Azure
{
   
    public class QueueMessage : IQueueMessage
    {
        private readonly IOptions<MyConfig> _config;
        public QueueMessage(IOptions<MyConfig> config)
        {
            _config = config;
        }
        public async Task<bool> AddMessage(QueueMessages message)
        {

            QueueClient queueClient = new QueueClient(_config.Value.QueueConnection, "datingappmessages");

            // Create the text message to add to the queue
           

            if (queueClient.Exists())
            {
                TimeSpan t1 = new TimeSpan(3, 0, 0);
                // Send a message to the queue
                var jsonMessage = JsonConvert.SerializeObject(message);
                string base64EncodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonMessage));

                await queueClient.SendMessageAsync(base64EncodedMessage);
            }
            return true;
        }
    }
}
