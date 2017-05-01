using System.Collections.Generic;
using System.Linq;

namespace RecruIT.Model.DBModel
{
    public class Departments
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Posts> Posts => GetPosts();



        public Departments()
        {
            if (DbLoader.IsDepartmentsEmpty())
                DbLoader.AddDefaultDepartments();
        }
        public Departments(int id, string name)
        {
            Id = id;
            Name = name;
        }
        private List<Posts> GetPosts()
        {
            using (var db = new HrContext())
            {
                return new List<Posts>(db.Posts.Where(x => x.DepartmentId == Id).ToList());
            }
        }


    }
}
