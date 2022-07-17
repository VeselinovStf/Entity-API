using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources(IConfigurationSection section)
        {
            List<ApiResource> resource = new List<ApiResource>();

            if (section != null)
            {
                List<ApiConfig> configs = new List<ApiConfig>();
                section.Bind("ApiResources", configs);
                foreach (var config in configs)
                {
                    resource.Add(new ApiResource(config.Name, config.DisplayName));
                }
            }

            return resource.ToArray();
        }

        internal static IEnumerable<ApiScope> GetScopes()
        {
            return new ApiScope[]
            {
                new ApiScope("identityAPIService", "Identity API"),
                new ApiScope("adminIdentityAPIService", "Admin Identity API")
            };
        }

        /// <summary>
        /// Define trusted Client client
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients(IConfigurationSection section)
        {
            List<Client> clients = new List<Client>();

            if (section != null)
            {
                List<ClientConfig> configs = new List<ClientConfig>();
                section.Bind("Clients", configs);
                foreach (var config in configs)
                {
                    Client client = new Client();

                    client.ClientId = config.ClientId;
                    client.ClientName = config.ClientId;

                    List<Secret> clientSecrets = new List<Secret>();

                    foreach (var secret in config.ClientSecrets)
                    {
                        clientSecrets.Add(new Secret(secret.Sha256()));
                    }

                    client.ClientSecrets = clientSecrets.ToArray();

                    GrantTypes grantTypes = new GrantTypes();
                    var allowedGrantTypes = grantTypes.GetType().GetProperty(config.AllowedGrantTypes);
                    client.AllowedGrantTypes = allowedGrantTypes == null ?
                        GrantTypes.ClientCredentials : (ICollection<string>)allowedGrantTypes.GetValue(grantTypes, null);

                    client.AllowedScopes = config.AllowedScopes.ToArray();

                    clients.Add(client);
                }
            }
            return clients.ToArray();
        }

    }

    public class ApiConfig
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class ClientConfig
    {
        public string ClientId { get; set; }
        public List<string> ClientSecrets { get; set; }
        public string AllowedGrantTypes { get; set; }
        public List<string> AllowedScopes { get; set; }
    }
}
