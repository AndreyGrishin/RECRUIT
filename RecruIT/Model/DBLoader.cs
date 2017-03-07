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
           new Departments(5,"Администраторы (Administrators)")
        };

        private static List<Posts> defaultPosts = new List<Posts>
        {
            new Posts("Разработчик (Developer)",1),
            new Posts("Системный администратор",5),
        };

        public static bool IsPostsEmpty()
        {
            using (var db = new HrContext())
            {
               return db.Posts.Any();
            }
        }
    }
}
