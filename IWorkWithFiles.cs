using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWithDepartments
{
    interface IWorkWithFiles
    {
        /// <summary>
        /// Запись json строки в файл
        /// </summary>
        void WriteToFile(string root, string fileName);
        /// <summary>
        /// Чтение json строки из файла
        /// </summary>
        string ReadFromFile(string fileName);
    }
}
