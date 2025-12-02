using System;
using IoTMonitor.ConsoleApp.Models;
using IoTMonitor.ConsoleApp.Services;
using IoTMonitor.ConsoleApp.Strategies;

namespace IoTMonitor.ConsoleApp.UI;

public class MenuHandler
{
    private readonly DeviceService _deviceService;

    public MenuHandler()
    {
        _deviceService = new DeviceService();
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- IoT Device Monitor ---");
            Console.WriteLine("1. Add Device");
            Console.WriteLine("2. List All Devices");
            Console.WriteLine("3. Update Device Status");
            Console.WriteLine("4. Search Device");
            Console.WriteLine("5. Sort Devices (by Name)");
            Console.WriteLine("6. System Health Report");
            Console.WriteLine("7. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddDevice();
                    break;
                case "2":
                    ListDevices();
                    break;
                case "3":
                    UpdateStatus();
                    break;
                case "4":
                    SearchDevice();
                    break;
                case "5":
                    SortDevices();
                    break;
                case "6":
                    _deviceService.GenerateReport();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private void AddDevice()
    {
        Console.WriteLine("\nSelect Device Type:");
        Console.WriteLine("1. Sensor");
        Console.WriteLine("2. Gateway");
        string typeChoice = Console.ReadLine();

        Console.Write("Enter ID: ");
        string id = Console.ReadLine();
        Console.Write("Enter Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter IP Address: ");
        string ip = Console.ReadLine();

        Device device = null;

        if (typeChoice == "1")
        {
            Console.Write("Enter Battery Level (0-100): ");
            if (double.TryParse(Console.ReadLine(), out double battery))
            {
                device = new Sensor(id, name, ip, DeviceStatus.Online, battery);
            }
            else
            {
                Console.WriteLine("Invalid battery level.");
                return;
            }
        }
        else if (typeChoice == "2")
        {
            Console.Write("Enter Connected Devices Count: ");
            if (int.TryParse(Console.ReadLine(), out int count))
            {
                device = new Gateway(id, name, ip, DeviceStatus.Online, count);
            }
            else
            {
                Console.WriteLine("Invalid count.");
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid device type.");
            return;
        }

        if (_deviceService.AddDevice(device))
        {
            Console.WriteLine("Device added successfully.");
        }
    }

    private void ListDevices()
    {
        var devices = _deviceService.GetAllDevices();
        Console.WriteLine("\n--- Device List ---");
        foreach (var device in devices)
        {
            Console.WriteLine(device);
        }
    }

    private void UpdateStatus()
    {
        Console.Write("Enter Device ID: ");
        string id = Console.ReadLine();
        
        Console.WriteLine("Select New Status:");
        Console.WriteLine("1. Online");
        Console.WriteLine("2. Offline");
        Console.WriteLine("3. Maintenance");
        string statusChoice = Console.ReadLine();

        DeviceStatus status;
        switch (statusChoice)
        {
            case "1": status = DeviceStatus.Online; break;
            case "2": status = DeviceStatus.Offline; break;
            case "3": status = DeviceStatus.Maintenance; break;
            default:
                Console.WriteLine("Invalid status.");
                return;
        }

        if (_deviceService.UpdateDeviceStatus(id, status))
        {
            Console.WriteLine("Status updated successfully.");
        }
    }

    private void SearchDevice()
    {
        Console.Write("Enter Device Name to Search: ");
        string name = Console.ReadLine();
        var device = _deviceService.SearchDevice(name);
        if (device != null)
        {
            Console.WriteLine("Device Found:");
            Console.WriteLine(device);
        }
        else
        {
            Console.WriteLine("Device not found.");
        }
    }

    private void SortDevices()
    {
        _deviceService.SortDevices(new BubbleSortStrategy());
        ListDevices();
    }
}
