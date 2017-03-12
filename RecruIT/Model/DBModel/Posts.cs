using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruIT.Model.DBModel
{
    public class Posts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        private Departments Department { get; set; }

        public Posts()
        {
            
        }
        public Posts(string name, int departmentId)
        {
            Name = name;
            DepartmentId = departmentId;
        }
    }
}
