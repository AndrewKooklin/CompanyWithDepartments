using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CompanyWithDepartments
{
    interface IWorkWithJson
    {
        /// <summary>
        /// Преобразование объектов "Клиент" к json строке
        /// </summary>
        //string ConvertToJsonDepartment(List<Department> departments);
        /// <summary>
        /// Преобразование объектов "Запись" к json строке
        /// </summary>
        string ConvertToJsonChanges(List<Change> dataChanges);
        /// <summary>
        /// Приведение json строки к объектам "Клиент"
        /// </summary>
        List<Department> ParseJsonDepartments(string root, Employee.Position position);
        /// <summary>
        /// Приведение json строки к объектам "Запись"
        /// </summary>
        List<Change> ParseJsonChanges(string root);
    }
}
