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
            while (true)
            {
                Console.WriteLine("What would you like to edit?");
                string input = Igets.Text("1. Username\n2.Location\n3.Balance\n4.Premium status\n5. Go back\n>> ");

                if (input.ToLower().Trim() == "")
                {
                    Console.WriteLine("Operation cancelled.");
                    return;
                }

                switch (input) // redo this
                {
                    case "1":
                        Username = Igets.Text("username >> ");
                        break;

                    case "2":
                        Location = Igets.Text("location >> ");
                        Country = Igets.Text("country >> ");
                        break;

                    case "3":
                        Balance = Igets.NumFloat("balance >> ");
                        break;

                    case "4":
                        bool premium = Igets.Bol("Is this profile premium? (y/n) >> ");

                        if (premium)
                        {
                            IsPremium = 1;
                            break;
                        }
                        else
                        {
                            IsPremium = 0;
                            break;
                        }

                    case "pass":
                    Passwordhash = BCrypt.Net.BCrypt.HashPassword(Igets.Text("Password: "));
                    break;

                    default:
                        return;
                }
            }
        }

        public bool Auth(string pass)
        {
            if (BCrypt.Net.BCrypt.Verify(pass, Passwordhash))
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