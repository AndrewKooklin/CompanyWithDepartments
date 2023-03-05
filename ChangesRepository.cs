using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CompanyWithDepartments
{
    public class ChangesRepository
    {
        private Window w;
        public List<Change> ChangesList { get; set; }

        public ChangesRepository(Window W)
        {
            this.w = W;
            this.ChangesList = new List<Change>();
        }
    }
}
