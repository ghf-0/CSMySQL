using MySql.Data.MySqlClient;
using Userdata;
using DatabaseQueries;
using Input;
using BCrypt.Net;
using MySqlDatabaseSettings;

// connection problems
namespace MySqlDatabase
{
    class Database
    {      

        private static readonly MySqlConnection Connection = new(ConnectionSettings.GetConnectionInfo()); // could be better
        public static void CreateTable()
        {
            try
            {
                using (var command = new MySqlCommand(Query.createTableQuery, Connection))
                {
                    Connection.Open();
                    command.ExecuteNonQuery();

                    Console.WriteLine("Table created.");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"ERROR!!******************\n{e}");
            }

            Connection.Close();
        }

        public static void SaveProfileData(Profile profile)
        {

            if (profile.Username == "")
            {
                Console.WriteLine("No profile loaded.");
                return;
            }
            try
            {
                Connection.Open();

                using (var command = new MySqlCommand(Query.saveProfileQuery, Connection))
                {
                    command.Parameters.AddWithValue("@username", profile.Username);
                    command.Parameters.AddWithValue("@location", profile.Location);
                    command.Parameters.AddWithValue("@country",profile.Country);
                    command.Parameters.AddWithValue("@balance", profile.Balance);
                    command.Parameters.AddWithValue(@"is_premium", profile.IsPremium);
                    command.Parameters.AddWithValue(@"hash_pass", profile.Passwordhash);
                    command.Parameters.AddWithValue(@"last_seen", DateTime.Now.ToString("HH:mm:ss"));

                    command.ExecuteNonQuery();
                    Console.WriteLine($"PROFILE {profile.Username} SAVED.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR LOL **********************************\n{e}");
            }

            Connection.Close();
        }

        public static void LoadProfileData(Profile profile)
        {
            string profileName = Igets.Text("Enter profile name: ");

            try
            {
                Connection.Open();

                using (var command = new MySqlCommand(Query.loadProfileQuery, Connection))
                {
                    command.Parameters.AddWithValue("@username",profileName);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string password = Igets.Text("Enter password: ");               //
                                                                                            //  This better go somewhere else
                            if (BCrypt.Net.BCrypt.Verify(password, reader.GetString(5)))    //
                            {
                                profile.Username = reader.GetString(0);
                                profile.Location = reader.GetString(1);
                                profile.Country = reader.GetString(2);
                                profile.Balance = reader.GetFloat(3);
                                profile.IsPremium = reader.GetInt32(4);
                                profile.Passwordhash = reader.GetString(5);

                                Console.WriteLine("Profile loaded.");
                            }
                            else
                            {
                                Console.WriteLine("Incorrect password.");
                            }

                        }
                        else
                        {
                            Console.WriteLine("User not found.");
                        }
                        reader.Close();
                    }
                }

                Connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e}");
            }
        }

        public static void ListProfiles()
        {
            Connection.Open();
            try
            {
                

                using (var command = new MySqlCommand(Query.listProfilesQuery, Connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"\n----------\n{reader.GetString(0)}\n{reader.GetString(1)}, {reader.GetString(2)}\nBalance: ${reader.GetFloat(3)}\n{(reader.GetInt32(4) == 1 ? "Premium" : "Standard")}\nLast seen: {reader.GetString(5)}\n----------");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"LOL ERROR: {e}");
            }
            Connection.Close();
        }

        public static void DeleteProfile()
        {

            string profileToDelete = Igets.Text("Enter profile name for deletion: ");
            Connection.Open();
            try
            {
                using (var command = new MySqlCommand(Query.deleteProfileQuery, Connection))
                {
                    command.Parameters.AddWithValue("@username", profileToDelete);

                    int rowCount = command.ExecuteNonQuery();

                    if (rowCount == 0)
                    {
                        Console.WriteLine($"No such profile: {profileToDelete}");
                        return;
                    }

                    Console.WriteLine($"{profileToDelete} data erased successfully.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e}");
            }
            Connection.Close();
        }

    }
}