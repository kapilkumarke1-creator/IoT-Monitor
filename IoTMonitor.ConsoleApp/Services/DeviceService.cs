using System;
using System.Collections.Generic;
using System.Linq;
using IoTMonitor.ConsoleApp.Models;
using IoTMonitor.ConsoleApp.Strategies;

namespace IoTMonitor.ConsoleApp.Services;

public class DeviceService
{
    private List<Device> _devices;
    private readonly DeviceRepository _repository;
    private readonly Logger _logger;

    public DeviceService(DeviceRepository repository = null)
    {
        _repository = repository ?? new DeviceRepository();
        _logger = Logger.Instance;
        _devices = _repository.LoadDevices();
    }

    public bool AddDevice(Device device)
    {
        if (_devices.Any(d => d.Id == device.Id))
        {
            Console.WriteLine("Error: Device with this ID already exists.");
            return false;
        }

        _devices.Add(device);
        _repository.SaveDevices(_devices);
        _logger.Log($"Device added: {device.Id}");
        return true;
    }

    public bool UpdateDeviceStatus(string id, DeviceStatus status)
    {
        var device = _devices.FirstOrDefault(d => d.Id == id);
        if (device == null)
        {
            Console.WriteLine("Error: Device not found.");
            return false;
        }

        device.Status = status;
        _repository.SaveDevices(_devices);
        _logger.Log($"Device status updated: {id} -> {status}");
        return true;
    }

    public Device SearchDevice(string name)
    {
        // Linear Search
        foreach (var device in _devices)
        {
            if (device.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return device;
            }
        }
        return null;
    }

    public void SortDevices(ISortStrategy strategy)
    {
        strategy.Sort(_devices);
        // We don't necessarily save after sorting, just display, but for consistency let's keep it in memory sorted.
        Console.WriteLine("Devices sorted.");
    }

    public List<Device> GetAllDevices()
    {
        return _devices;
    }
    
    public void GenerateReport()
    {
         // Simple report generation logic
         var onlineCount = _devices.Count(d => d.Status == DeviceStatus.Online);
         var offlineCount = _devices.Count(d => d.Status == DeviceStatus.Offline);
         var maintenanceCount = _devices.Count(d => d.Status == DeviceStatus.Maintenance);
         
         Console.WriteLine("\n--- System Health Report ---");
         Console.WriteLine($"Total Devices: {_devices.Count}");
         Console.WriteLine($"Online: {onlineCount}");
         Console.WriteLine($"Offline: {offlineCount}");
         Console.WriteLine($"Maintenance: {maintenanceCount}");
         Console.WriteLine("----------------------------\n");
    }
}
