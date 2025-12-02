using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using IoTMonitor.ConsoleApp.Models;

namespace IoTMonitor.ConsoleApp.Services;

public class DeviceRepository
{
    private readonly string _filePath;
    private readonly Logger _logger;

    public DeviceRepository(string filePath = "devices.json")
    {
        _filePath = filePath;
        _logger = Logger.Instance;
    }

    public List<Device> LoadDevices()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Device>();
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<Device>();

            return JsonSerializer.Deserialize<List<Device>>(json) ?? new List<Device>();
        }
        catch (Exception ex)
        {
            _logger.Log($"Error loading devices: {ex.Message}");
            Console.WriteLine("Error loading data. Starting with empty list.");
            return new List<Device>();
        }
    }

    public void SaveDevices(List<Device> devices)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(devices, options);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            _logger.Log($"Error saving devices: {ex.Message}");
            Console.WriteLine("Error saving data.");
        }
    }
}
