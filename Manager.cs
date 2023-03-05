using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWithDepartments
{
    public class Manager : Employee, IManager
    {
        public Manager()
        {

        }
        /// <summary>
        /// Создание нового клиента
        /// </summary>
        /// <returns></returns>
        public Client AddClient(string firstName, string lastName,
                        string fathersName, long phone,
                        string passportNumber)
        {
            Client client = new Client(firstName, lastName,
                                    fathersName, phone,
                                    passportNumber);

            return client;
        }
    }
}
