using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Azure
{
    public interface IKeyVault
    {
        Task<string> GetKeyValue(string keyName);
    }
}
