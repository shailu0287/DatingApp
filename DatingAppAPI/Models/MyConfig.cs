using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Models
{
    public class MyConfig
    {
        public string StorageConnection { get; set; }
        public string Container { get; set; }

        public string QueueConnection { get; set; }
    }
}
