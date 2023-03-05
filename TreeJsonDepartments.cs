using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CompanyWithDepartments
{
    /// <summary>
    /// Создание дерева хранения департаментов
    /// </summary>

    [Serializable]
    [JsonObject]
    public class TreeJsonDepartments
    {
        [JsonConstructor]
        public TreeJsonDepartments(List<Department> departments)
        {
            Departments = departments;
        }

        [JsonProperty]
        public List<Department> Departments { get; set; }

    }
}
