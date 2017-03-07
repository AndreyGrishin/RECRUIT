using System.Collections.Generic;

namespace RecruIT.Model.DBModel
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public string LinkedIn { get; set; }


        public List<Employees> Employees { get; set; }

    }
}
 