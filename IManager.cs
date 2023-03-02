using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWithDepartments
{
    interface IManager
    {
        Client AddClient(string firstName, string lastName,
                        string fathersName, long phone,
                        string passportNumber);
    }
}
