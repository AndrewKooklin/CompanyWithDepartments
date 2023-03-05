using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CompanyWithDepartments
{
    interface IGetRecordsFromBase
    {
        /// <summary>
        /// Получение списка клиентов из файла
        /// </summary>
        List<Department> GetDepartmentsItemSourse(Employee.Position position);
        /// <summary>
        /// Получение списка записей изменений из файла
        /// </summary>
        List<Change> GetChangesItemSourse();
    }
}
