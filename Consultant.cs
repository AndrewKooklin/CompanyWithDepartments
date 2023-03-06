using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CompanyWithDepartments
{
    public class Consultant : Employee, IConsultant
    {
        public Consultant()
        {

        }

        public List<Client> GetClientsCopy(List<Client> clients, Employee.Position position)
        {
            List<Client> newClientsCopy = new List<Client>();

            foreach (var item in clients)
            {
                newClientsCopy.Add((Client)item.Clone());
            }

            foreach (var item in newClientsCopy)
            {
                item.PassportNumber = item.GetPassportNumber(position);
            }

            return newClientsCopy;
        }
    }
}
