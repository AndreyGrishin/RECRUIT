using System;

namespace RecruIT.Model.DBModel
{
    public class Employees
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Post { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int ContactsInfoId { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public DateTime StartDate { get; set; }

        public Employees() { }

        public Employees(int id, string firstName, string middleName, string lastName, string post, string gender, DateTime birthDate, ContactInfo contactInfo, DateTime startDate)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Post = post;
            Gender = gender;
            BirthDate = birthDate;
            ContactInfo = contactInfo;
            StartDate = startDate;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6}", Id, FirstName, MiddleName, LastName, Post, Gender, BirthDate);
        }
    }
}
