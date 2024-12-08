using Input;
using Userdata;
using MySqlDatabase;

namespace CSMySQL
{
    class Program
    {
        public static Profile CurrentProfile = new("", "", "", "", 0, 0);
        public static Dictionary<string, Action> Commands = new Dictionary<string, Action> {
            {"exit", () => Environment.Exit(0)},
            {"save", () => Database.SaveProfileData(CurrentProfile)},
            {"load", () => Database.LoadProfileData(CurrentProfile)},
            {"delete", Database.DeleteProfile},
            {"list", Database.ListProfiles},
            {"edit", CurrentProfile.EditInfo},
            {"info", CurrentProfile.DisplayData},
            {"newtable", Database.CreateTable}
        };
        static void InvokeCommandInput(string commandInput)
        {
            if (Commands.TryGetValue(commandInput.ToLower().Trim(), out Action? action) && action != null)
            {
                action();
                return;
            }

            Console.WriteLine("No such command");
        }
        static void Main(string[] args)
        {
            
            while (true)
            {
                string input = Igets.Text(">> ");
                InvokeCommandInput(input);
            }
        }
    }
}