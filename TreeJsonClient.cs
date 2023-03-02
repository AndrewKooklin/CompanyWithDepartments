using System;
using System.Collections.ObjectModel;

namespace CompanyWithDepartments
{
    public class TreeJsonClient
    {
        /// <summary>
        /// Создание дерева хранения клиентов
        /// </summary>
        public TreeJsonClient(ObservableCollection<Client> clientsList)
        {
            ClientsList = new ObservableCollection<Client>();
            ClientsList = clientsList;
        }

        public ObservableCollection<Client> ClientsList { get; set; }

    }
}
