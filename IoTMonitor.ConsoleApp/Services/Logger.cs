using System;
using System.IO;

namespace IoTMonitor.ConsoleApp.Services;

public class Logger
{
    private static Logger _instance;
    private static readonly object _lock = new object();
    private readonly string _logFilePath = "log.txt";

    private Logger() { }

    public static Logger Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
                return _instance;
            }
        }
    }

    public void Log(string message)
    {
        string logEntry = $"{DateTime.Now}: {message}";
        try
        {
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write to log file: {ex.Message}");
        }
    }
}
