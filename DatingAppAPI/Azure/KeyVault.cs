using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
namespace DatingAppAPI.Azure
{
    public class KeyVault : IKeyVault
    {
        string vaultAddress = "https://dapkey.vault.azure.net/";

        public async Task<string> GetKeyValue(string keyName)
        {
            var client = new SecretClient(vaultUri: new Uri(vaultAddress), credential: new DefaultAzureCredential());

            KeyVaultSecret secret =await client.GetSecretAsync(keyName);
            return secret.Value;
        }
    }
}
