namespace BookStore.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        private string _password = string.Empty;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        public string PhoneNumber { get; set; }
        public static int UserCount { get; set; }

        public User(string fullname, string email, string password, string phoneNumber)
        {
            UserCount++;
            Id = UserCount;
            Fullname = fullname;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }

        public override string ToString()
        {
            return $"{Id}|{Fullname}|{Email}|{PhoneNumber}";
        }
    }
}
