namespace DatabaseQueries
{
    class Query
    {
        public static string createTableQuery = @"
        CREATE TABLE IF NOT EXISTS profiles (
        id INTEGER PRIMARY KEY AUTO_INCREMENT,
        username VARCHAR(25),
        location VARCHAR(25),
        country VARCHAR(15),
        balance REAL,
        is_premium INTEGER,
        hash_pass TEXT,
        last_seen VARCHAR(8)
        );
        ";

        public static string dropTableQuery = @"
        DROP TABLE profiles;
        ";

        public static string saveProfileQuery = @"
        INSERT INTO profiles (username, location, country,  balance, is_premium, hash_pass, last_seen)
        VALUES (@username, @location, @country, @balance, @is_premium, @hash_pass, @last_seen);
        ";

        public static string loadProfileQuery = @"
        SELECT username, location, country, balance, is_premium, hash_pass FROM profiles WHERE username = @username
        ";

        public static string listProfilesQuery = @"SELECT username, location, country, balance, is_premium, last_seen FROM profiles";
        
        public static string updateProfileQuery = @"
        UPDATE profiles SET username = @username, location = @location, country = @country, balance = @balance, is_premium = @is_premium, last_seen = @last_seen
        WHERE username = @name
        "; // HUGE mistake. Should've included ID into the profile class since the beginning

        public static string deleteProfileQuery = @"DELETE FROM profiles WHERE username = @username";
    }
}