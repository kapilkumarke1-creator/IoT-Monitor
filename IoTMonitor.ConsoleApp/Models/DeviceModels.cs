using System.Text.Json.Serialization;

namespace IoTMonitor.ConsoleApp.Models;

public enum DeviceStatus
{
    Online,
    Offline,
    Maintenance
}

[JsonDerivedType(typeof(Sensor), typeDiscriminator: "Sensor")]
[JsonDerivedType(typeof(Gateway), typeDiscriminator: "Gateway")]
public abstract class Device
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string IpAddress { get; set; }
    public DeviceStatus Status { get; set; }
    public abstract string Type { get; }

    public Device(string id, string name, string ipAddress, DeviceStatus status)
    {
        Id = id;
        Name = name;
        IpAddress = ipAddress;
        Status = status;
    }

    public override string ToString()
    {
        return $"[{Type}] ID: {Id}, Name: {Name}, IP: {IpAddress}, Status: {Status}";
    }
}

public class Sensor : Device
{
    public double BatteryLevel { get; set; }
    public override string Type => "Sensor";

    public Sensor(string id, string name, string ipAddress, DeviceStatus status, double batteryLevel)
        : base(id, name, ipAddress, status)
    {
        BatteryLevel = batteryLevel;
    }

    public override string ToString()
    {
        return base.ToString() + $", Battery: {BatteryLevel}%";
    }
}

public class Gateway : Device
{
    public int ConnectedDevicesCount { get; set; }
    public override string Type => "Gateway";

    public Gateway(string id, string name, string ipAddress, DeviceStatus status, int connectedDevicesCount)
        : base(id, name, ipAddress, status)
    {
        ConnectedDevicesCount = connectedDevicesCount;
    }

    public override string ToString()
    {
        return base.ToString() + $", Connected Devices: {ConnectedDevicesCount}";
    }
}
