using System.Collections.ObjectModel;

namespace CompanyWithDepartments
{
    interface IWorkWithJson
    {
        /// <summary>
        /// Преобразование объектов "Клиент" к json строке
        /// </summary>
        string ConvertToJsonClients(ObservableCollection<Client> clients);
        /// <summary>
        /// Преобразование объектов "Запись" к json строке
        /// </summary>
        string ConvertToJsonChanges(ObservableCollection<Change> dataChanges);
        /// <summary>
        /// Приведение json строки к объектам "Клиент"
        /// </summary>
        ObservableCollection<Client> ParseJsonClients(string root, Employee.Position position);
        /// <summary>
        /// Приведение json строки к объектам "Запись"
        /// </summary>
        ObservableCollection<Change> ParseJsonChanges(string root);
    }
}
