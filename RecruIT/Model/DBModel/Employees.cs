using System;
using System.Linq;
using Windows.UI.Xaml.Media.Imaging;

namespace RecruIT.Model.DBModel
{
    public class Employees
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int ContactsInfoId { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public int PostId { get; set; }
        public DateTime StartDate { get; set; }
        public string PostName => GetPostName();

        public string PhotoPath { get; set; }

        private string GetPostName()
        {
            using (var db = new HrContext())
            {
                return db.Posts.Where(x => x.Id == PostId).ToList().Select(item => item.Name).FirstOrDefault();
            }
        }

        public Employees()
        {
            
        }

        public Employees(int id, string firstName, string middleName, string lastName, int postId, string gender, DateTime birthDate, ContactInfo contactInfo, DateTime startDate, string photoPath)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PostId = postId;
            Gender = gender;
            BirthDate = birthDate;
            ContactInfo = contactInfo;
            StartDate = startDate;
            PhotoPath = photoPath;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6}", Id, FirstName, MiddleName, LastName, PostId, Gender, BirthDate);
        }
    }
}
