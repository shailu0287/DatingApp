using DatingApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Azure
{
    public interface IQueueMessage
    {
        Task<bool> AddMessage(QueueMessages message);
    }
}
