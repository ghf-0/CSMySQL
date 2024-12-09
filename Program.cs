using Input;
using Commands;

namespace CSMySQL
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            while (true)
            {
                string input = Igets.Text(">> ");
                Command.InvokeCommandInput(input);
            }
        }
    }
}