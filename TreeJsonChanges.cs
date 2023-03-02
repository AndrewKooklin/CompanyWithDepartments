using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWithDepartments
{
    class TreeJsonChanges
    {
        /// <summary>
        /// Создание дерева хранения записей
        /// </summary>
        public TreeJsonChanges(ObservableCollection<Change> dataChangeList)
        {
            DataChangeList = new ObservableCollection<Change>();
            DataChangeList = dataChangeList;
        }

        public ObservableCollection<Change> DataChangeList { get; set; }
    }
}
