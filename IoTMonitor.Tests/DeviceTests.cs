using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IoTMonitor.ConsoleApp.Models;
using IoTMonitor.ConsoleApp.Services;
using IoTMonitor.ConsoleApp.Strategies;
using Xunit;

namespace IoTMonitor.Tests;

public class DeviceTests : IDisposable
{
    private readonly string _testFilePath;
    private readonly DeviceService _service;

    public DeviceTests()
    {
        _testFilePath = $"test_devices_{Guid.NewGuid()}.json";
        var repository = new DeviceRepository(_testFilePath);
        _service = new DeviceService(repository);
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Fact]
    public void AddDevice_ShouldAddDevice_WhenDeviceIsNew()
    {
        var device = new Sensor("S1", "Temp Sensor", "192.168.1.10", DeviceStatus.Online, 80);
        bool result = _service.AddDevice(device);

        Assert.True(result);
        Assert.Single(_service.GetAllDevices());
    }

    [Fact]
    public void AddDevice_ShouldFail_WhenIdExists()
    {
        var device1 = new Sensor("S1", "Temp Sensor", "192.168.1.10", DeviceStatus.Online, 80);
        _service.AddDevice(device1);

        var device2 = new Gateway("S1", "Gateway 1", "192.168.1.1", DeviceStatus.Online, 5);
        bool result = _service.AddDevice(device2);

        Assert.False(result);
        Assert.Single(_service.GetAllDevices());
    }

    [Fact]
    public void UpdateStatus_ShouldUpdate_WhenDeviceExists()
    {
        var device = new Sensor("S1", "Temp Sensor", "192.168.1.10", DeviceStatus.Online, 80);
        _service.AddDevice(device);

        bool result = _service.UpdateDeviceStatus("S1", DeviceStatus.Offline);

        Assert.True(result);
        Assert.Equal(DeviceStatus.Offline, _service.SearchDevice("Temp Sensor").Status);
    }

    [Fact]
    public void SearchDevice_ShouldReturnDevice_WhenNameMatches()
    {
        var device = new Sensor("S1", "Temp Sensor", "192.168.1.10", DeviceStatus.Online, 80);
        _service.AddDevice(device);

        var found = _service.SearchDevice("Temp Sensor");

        Assert.NotNull(found);
        Assert.Equal("S1", found.Id);
    }

    [Fact]
    public void SortDevices_ShouldSortByName()
    {
        _service.AddDevice(new Sensor("S1", "Zebra Sensor", "1.1.1.1", DeviceStatus.Online, 10));
        _service.AddDevice(new Sensor("S2", "Alpha Sensor", "1.1.1.2", DeviceStatus.Online, 20));
        _service.AddDevice(new Sensor("S3", "Beta Sensor", "1.1.1.3", DeviceStatus.Online, 30));

        _service.SortDevices(new BubbleSortStrategy());

        var devices = _service.GetAllDevices();
        Assert.Equal("Alpha Sensor", devices[0].Name);
        Assert.Equal("Beta Sensor", devices[1].Name);
        Assert.Equal("Zebra Sensor", devices[2].Name);
    }
}
