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
                Console.WriteLine($"ERROR CREATING TABLE:\n{e}");
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

            Connection.Open();
            try
            {
                using (var command = new MySqlCommand(Query.saveProfileQuery, Connection))
                {
                    var data = new Dictionary<string, object>
                    {
                        {"@username", profile.Username},
                        {"@location", profile.Location},
                        {"@country", profile.Country},
                        {"@balance", profile.Balance},
                        {"@is_premium", profile.IsPremium},
                        {"@hash_pass", profile.Passwordhash},
                        {"@last_seen", DateTime.Now.ToString("HH:mm:ss")}
                    };

                    foreach (var row in data)
                    {
                        command.Parameters.AddWithValue(row.Key, row.Value);
                    }
                    
                    command.ExecuteNonQuery();
                    Console.WriteLine($"PROFILE {profile.Username} SAVED.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"There was an error: \n{e}");
            }
            finally
            {
                Connection.Close();
            }
            
        }

        public static void LoadProfileData(Profile profile)
        {
            string profileName = Igets.Text("Enter profile name: ");
            Connection.Open();

            try
            {
                using (var command = new MySqlCommand(Query.loadProfileQuery, Connection))
                {
                    command.Parameters.AddWithValue("@username",profileName);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            Console.WriteLine("User not found.");
                            return;
                        }

                        if (!profile.Auth(reader.GetString(6)))
                        {
                            Console.WriteLine("Incorrect Password.");
                            return;
                        }

                        profile.Id = reader.GetInt32(0);
                        profile.Username = reader.GetString(1);
                        profile.Location = reader.GetString(2);
                        profile.Country = reader.GetString(3);
                        profile.Balance = reader.GetFloat(4);
                        profile.IsPremium = reader.GetInt32(5);
                        profile.Passwordhash = reader.GetString(6);

                        Console.WriteLine("Profile loaded.");
                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e}");
            }
            finally
            {
                Connection.Close();
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
            finally
            {
                Connection.Close();
            }
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
            finally
            {
                Connection.Close();
            }
        }

        public static void UpdateProfile(Profile profile)
        {
            Connection.Open();

            using (var command = new MySqlCommand(Query.updateProfileQuery, Connection))
            {
                var data = new Dictionary<string, object>
                {
                    {"@id", profile.Id}, // not good
                    {"@username", profile.Username},
                    {"@location", profile.Location},
                    {"@country", profile.Country},
                    {"@balance", profile.Balance},
                    {"@is_premium", profile.IsPremium},
                    {"@last_seen", DateTime.Now.ToString("HH:mm:ss")}
                };

                foreach (var row in data)
                {
                    command.Parameters.AddWithValue(row.Key, row.Value);
                }

                var rows = command.ExecuteNonQuery();
                
                if (rows == 0)
                {
                    Console.WriteLine("There is no profile with the provided ID.");
                    Connection.Close();
                    return;
                }

                Console.WriteLine("Profile data updated.");
                Connection.Close();
                return;
            }
        }

    }
}