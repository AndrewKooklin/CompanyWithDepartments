using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWithDepartments
{
    public interface IConsultant
    {
        /// <summary>
        /// Получение копии списка клиентов департамента
        /// </summary>
        List<Client> GetClientsCopy(List<Client> clients, Employee.Position position);
    }
}
