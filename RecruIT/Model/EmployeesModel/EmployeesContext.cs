using Microsoft.EntityFrameworkCore;
using RecruIT.Model.EmployeesModel;

namespace RecruIT.Model.Employyes
{
   
    internal class EmployeesContext : DbContext
    {
        public DbSet<Employees> Employees { get; set; }
        public DbSet<ContactInfo> ContectInfo { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EmployeesDB;Trusted_Connection=True;");
        }
    }
}
