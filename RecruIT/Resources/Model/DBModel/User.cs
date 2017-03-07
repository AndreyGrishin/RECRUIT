namespace RecruIT.Model.DBModel
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public User()
        {
            
        }
        public User(int id, string login, string name, string password)
        {
            Id = id;
            Login = login;
            Name = name;
            Password = password;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Id, Login, Name, Password);
        }
    }
}
