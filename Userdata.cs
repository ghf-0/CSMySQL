using BCrypt.Net;
using Input;

namespace Userdata
{
    class Profile(string username, string passwordhash, string location, string country, float balance, int ispremium)
    {
        public int? Id { get; set; }
        public string Username { get; set; } = username;
        public string Passwordhash { get; set; } = BCrypt.Net.BCrypt.HashPassword(passwordhash);
        public string Location { get; set; } = location;
        public string Country { get; set; } = country;
        public float Balance { get; set; } = balance;
        public int IsPremium { get; set; } = ispremium;

        public void DisplayData()
        {
            if (Username.Trim() == "")
            {
                Console.WriteLine("No information available");
                return;
            }

            Console.WriteLine($"{Username.ToUpper()}\n{Location}, {Country},{Balance}, {(IsPremium == 1 ? "Premium" : "Not premium")}");
        }

        public void EditInfo()
        {
            Username = Igets.Text("Enter username: ");
            Location = Igets.Text("Enter location: ");
            Country = Igets.Text("Enter country: ");
            Balance = Igets.NumFloat("Enter balance: ");
            Passwordhash = BCrypt.Net.BCrypt.HashPassword(Igets.Text("Enter new password: ")); // doesn't work as intended
            // Console.WriteLine(Passwordhash);
            
        }

        public bool Auth(string hash) // not good
        {
            Console.WriteLine(Passwordhash);
            string pass = Igets.Text("Enter password: ");
            
            if (BCrypt.Net.BCrypt.Verify(pass, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}