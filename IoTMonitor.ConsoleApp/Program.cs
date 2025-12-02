using IoTMonitor.ConsoleApp.UI;

namespace IoTMonitor.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        MenuHandler menuHandler = new MenuHandler();
        menuHandler.ShowMenu();
    }
}
