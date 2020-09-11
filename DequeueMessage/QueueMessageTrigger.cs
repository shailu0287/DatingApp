using System;
using System.Data.SqlClient;
using AutoMapper;
using DatingApp.Data.Models;
using DatingApp.Data.Repository;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DequeueMessage
{
    public static class QueueMessageTrigger
    {

        [FunctionName("QueueMessageTrigger")]
        public static void Run([QueueTrigger("datingappmessages", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
             var message = JsonConvert.DeserializeObject<QueueMessages>(myQueueItem);
                using (SqlConnection conn = new SqlConnection("Server=tcp:shailensqlserver123.database.windows.net,1433;Initial Catalog=DatingApp;Persist Security Info=False;User ID=shailen;Password=DatingApp@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    conn.Open();
                    string text = "insert into messages (SenderId,RecipientId,Content,IsRead,MessageSent,SenderDeleted,RecipientDeleted)  values  (" + message.SenderId + "," + message.RecipientId + ", '" + message.Content + "','false', '" + message.MessageSent + "','" + message.SenderDeleted + "','" + message.RecipientDeleted + "' ) ";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        // Execute the command and log the # rows affected.
                        int rows = cmd.ExecuteNonQuery();
                        log.LogInformation($"{rows} rows were updated");
                    }
                }
        }

    }
}
