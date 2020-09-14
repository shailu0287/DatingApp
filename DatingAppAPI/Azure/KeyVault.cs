using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using Microsoft.Identity.Client;

namespace DatingAppAPI.Azure
{
    public class KeyVault : IKeyVault
    {
        string vaultAddress = "https://dapkey.vault.azure.net/";

        public async Task<string> GetKeyValue(string keyName)
        {
            try
            {
                SecretClientOptions options = new SecretClientOptions()
                {
                    Retry =
                        {
                            Delay= TimeSpan.FromSeconds(2),
                            MaxDelay = TimeSpan.FromSeconds(16),
                            MaxRetries = 5,
                            Mode = RetryMode.Exponential
                         }
                };
                var client = new SecretClient(new Uri(vaultAddress), new DefaultAzureCredential(), options);

                KeyVaultSecret secret = await client.GetSecretAsync("key1");

                string secretValue = secret.Value;
                return secretValue;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
