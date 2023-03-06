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
    public class Client : ICloneable
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

        /// <summary>
        /// Создание копии объекта "Client"
        /// </summary>
        public object Clone()
        {
            return new Client
            {
                LastName = this.LastName,
                FirstName = this.FirstName,
                FathersName = this.FathersName,
                Phone = this.Phone,
                PassportNumber = this.PassportNumber
            };
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

        /// <summary>
        /// Сортировка по фамилии и имени
        /// </summary>
        public class SortByLastName : IComparer<Client>
        {
            public int Compare(Client x, Client y)
            {
                int compareLastName = x.LastName.CompareTo(y.LastName);
                if (compareLastName == 0)
                {
                    return x.FirstName.CompareTo(y.FirstName);
                }
                return compareLastName;
            }
        }

        /// <summary>
        /// Сортировка по имени и отчеству
        /// </summary>
        public class SortByFirstName : IComparer<Client>
        {
            public int Compare(Client x, Client y)
            {
                int compareFirstName = x.FirstName.CompareTo(y.FirstName);
                if (compareFirstName == 0)
                {
                    return x.FathersName.CompareTo(y.FathersName);
                }
                return compareFirstName;
            }
        }

        /// <summary>
        /// Сортировка по номеру телефона
        /// </summary>
        public class SortByPhone : IComparer<Client>
        {
            public int Compare(Client x, Client y)
            {
                int comparePhoneNumber = x.Phone.CompareTo(y.Phone);
                return comparePhoneNumber;
            }
        }

        /// <summary>
        /// Сортировка по номеру номеру паспорта
        /// </summary>
        public class SortByPassportNumber : IComparer<Client>
        {
            public int Compare(Client x, Client y)
            {
                int comparePassportNumber = x.PassportNumber.CompareTo(y.PassportNumber);
                return comparePassportNumber;
            }
        }
    }
}
