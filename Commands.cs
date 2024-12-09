using MySqlDatabaseSettings;
using MySqlDatabase;

namespace Commands
{
    class Command
    {
        public static Dictionary<string, Action> Commands = new Dictionary<string, Action> {
            {"exit", () => Environment.Exit(0)},
            {"save", () => Database.SaveProfileData(ConnectionSettings.CurrentProfile)},
            {"load", () => Database.LoadProfileData(ConnectionSettings.CurrentProfile)},
            {"update", () => Database.UpdateProfile(ConnectionSettings.CurrentProfile)},
            {"delete", Database.DeleteProfile},
            {"list", Database.ListProfiles},
            {"edit", ConnectionSettings.CurrentProfile.EditInfo},
            {"info", ConnectionSettings.CurrentProfile.DisplayData},
            {"newtable", Database.CreateTable}
        };
        public static void InvokeCommandInput(string commandInput)
        {
            if (Commands.TryGetValue(commandInput.ToLower().Trim(), out Action? action) && action != null)
            {
                action();
                return;
            }

            Console.WriteLine("No such command");
        }
    }
}