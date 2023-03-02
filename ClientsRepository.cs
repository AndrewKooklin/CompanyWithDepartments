using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CompanyWithDepartments
{
    public class ClientsRepository
    {
        private Window w;
        public ObservableCollection<Client> ClientsList { get; set; }

        public ClientsRepository(Window W)
        {
            this.w = W;
            this.ClientsList = new ObservableCollection<Client>();
        }
    }
}
