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
        public TreeJsonChanges(List<Change> dataChangeList)
        {
            DataChangeList = new List<Change>();
            DataChangeList = dataChangeList;
        }

        public List<Change> DataChangeList { get; set; }
    }
}
