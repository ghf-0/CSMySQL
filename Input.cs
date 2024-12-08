namespace Input
{
    class Igets
    {
        static public string Text(string msg)
        {
            while (true)
            {
                Console.Write(msg);
                string input = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                
                Console.WriteLine("Input must not be empty space");
            }
        }

        static public int NumInt(string msg)
        {
            while (true)
            {
                string input = Text(msg);

                if (int.TryParse(input, out int parsed))
                {
                    return parsed;
                }

                Console.WriteLine("Input must be a number.");
            }
        }

        static public float NumFloat(string msg)
        {
            while (true)
            {
                string input = Text(msg);

                if (float.TryParse(input, out float parsed))
                {
                    return parsed;
                }

                Console.WriteLine("Input must be a real number.");
            }
        }

        static public bool Bol(string msg)
        {
            while (true)
            {
                string input = Text(msg).ToLower();

                if (input == "y")
                {
                    return true;
                }

                if (input == "n")
                {
                    return false;
                }

                Console.WriteLine("Invalid input");
            }
        }
    }
}