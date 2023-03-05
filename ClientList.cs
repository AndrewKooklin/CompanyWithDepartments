using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CompanyWithDepartments
{
    [Serializable]
    [JsonObject]
    public class ClientList
    {
        [JsonConstructor]
        public ClientList(List<Client> clients)
        {
            Clients = clients;
        }

        [JsonProperty]
        public List<Client> Clients { get; set; }
    }
}
