using System.Collections.ObjectModel;

namespace CompanyWithDepartments
{
    interface IGetRecordsFromBase
    {
        /// <summary>
        /// Получение списка клиентов из файла
        /// </summary>
        ObservableCollection<Client> GetClietsItemSourse(Employee.Position position);
        /// <summary>
        /// Получение списка записей из файла
        /// </summary>
        ObservableCollection<Change> GetChangesItemSourse();
    }
}
