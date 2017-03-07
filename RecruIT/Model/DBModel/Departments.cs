using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruIT.Model.DBModel
{
    public class Departments
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Posts> Posts { get; set; }

        public Departments(string name)
        {
            Name = name;
        }
        public Departments(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
