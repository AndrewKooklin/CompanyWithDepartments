using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CompanyWithDepartments
{
    [Serializable]
    [JsonObject]
    public class Department
    {
        
        [NonSerialized]
        public string nameDepartment;

        //[JsonConstructor]
        public Department(string nameDepartment)
        {
            this.NameDepartment = nameDepartment;
        }

        [JsonConstructor]
        public Department(string nameDepartment, List<Client> clients)
        {
            this.NameDepartment = nameDepartment;
            Clients = clients;
        }

        [JsonProperty]
        public string NameDepartment { get; set; }

        [JsonProperty]
        public List<Client> Clients { get; set; }

        
    }
}
