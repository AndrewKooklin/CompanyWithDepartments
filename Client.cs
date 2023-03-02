using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWithDepartments
{
    [Serializable]
    [JsonObject]
    public class Client
    {
        [NonSerialized]
        private string passportNumber;

        [JsonProperty]
        public string LastName { get; set; }
        [JsonProperty]
        public string FirstName { get; set; }
        [JsonProperty]
        public string FathersName { get; set; }
        [JsonProperty]
        public long Phone { get; set; }
        [JsonProperty]
        public string PassportNumber
        {
            get
            {
                return passportNumber;
            }
            set
            {
                passportNumber = value;
            }
        }

        public string GetPassportNumber(Employee.Position type)
        {
            string passport;
            switch (type)
            {
                case Employee.Position.Consultant:
                    {
                        passport = "****-******";
                        break;
                    }
                case Employee.Position.Manager:
                    {
                        passport = passportNumber;
                        break;
                    }
                default:
                    {
                        passport = "";
                        break;
                    }
            }

            return passport;
        }

        [JsonConstructor]
        public Client(string firstName, string lastName,
                        string fathersName, long phone,
                        string passportNumber)
        {
            this.LastName = lastName;
            this.FirstName = firstName;
            this.FathersName = fathersName;
            this.Phone = phone;
            this.PassportNumber = passportNumber;
        }

        public Client()
        {

        }
    }
}
