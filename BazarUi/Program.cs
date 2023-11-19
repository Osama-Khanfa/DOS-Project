using BazarUi.Utilties;

namespace BazarUi
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Operations operations = new Operations();
            Menu mainMenu= new Menu(operations);
            mainMenu.Run();
           
        }
    }
}