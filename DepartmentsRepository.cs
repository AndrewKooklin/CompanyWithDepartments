using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CompanyWithDepartments
{
    public class DepartmentsRepository
    {
        private Window w;
        public List<Department> Departments { get; set; }

        public DepartmentsRepository(Window W)
        {
            this.w = W;
            this.Departments = new List<Department>();
        }
    }
}
