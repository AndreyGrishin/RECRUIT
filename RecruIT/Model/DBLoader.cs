using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecruIT.Model.DBModel;

namespace RecruIT.Model
{
    public static class DbLoader
    {
        private static List<Departments> defaultDepartments = new List<Departments>
        {
           new Departments(1,"Разработчики (Developers)"),
           new Departments(2,"Дизайнеры (Designers)"),
           new Departments(3,"QA (Quality Assurance)"),
           new Departments(4,"Менеджеры (Managers)"),
           new Departments(5,"Администраторы (Administrators)"),
           new Departments(6, "Управление персоналом (HR)")
        };

        private static List<Posts> defaultPosts = new List<Posts>
        {
            new Posts("Разработчик (Developer)",1),
            new Posts("Системный администратор",5),
            new Posts("Тестировщик",3),
            new Posts("HR менеджер",6),
            new Posts("Специалист по привлечению клиентов",4),
            new Posts("Дизайнер UI/UX интерфейсов",2)
        };

        public static bool IsPostsEmpty()
        {
            using (var db = new HrContext())
            {
                return db.Posts.Count() == 0;
            }
        }
        public static bool IsDepartmentsEmpty()
        {
            using (var db = new HrContext())
            {
                return db.Departments.Count() == 0;
            }
        }

        public static void AddDefaultPosts()
        {           
            using (var db = new HrContext())
            {
                db.Posts.AddRangeAsync(defaultPosts);
                db.SaveChangesAsync();
            }         
        }

        public static void AddDefaultDepartments()
        {
            using (var db = new HrContext())
            {
                db.Departments.AddRangeAsync(defaultDepartments);
                db.SaveChangesAsync();
            }
        }

        internal static List<Posts> GetPosts(int departmentId)
        {
            using (var db = new HrContext())
            {
                return new List<Posts>(db.Posts.Where(x => x.DepartmentId == departmentId).ToList());
            }
        }

        public static List<Departments> GetDepartments()
        {
            using (var db = new HrContext())
            {
                return new List<Departments>(db.Departments.ToList());
            }
        }

    }
}
