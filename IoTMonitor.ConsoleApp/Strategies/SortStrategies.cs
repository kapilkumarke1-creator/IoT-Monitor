using System.Collections.Generic;
using IoTMonitor.ConsoleApp.Models;

namespace IoTMonitor.ConsoleApp.Strategies;

public interface ISortStrategy
{
    void Sort(List<Device> devices);
}

public class BubbleSortStrategy : ISortStrategy
{
    public void Sort(List<Device> devices)
    {
        int n = devices.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                // Sort by Name ascending
                if (string.Compare(devices[j].Name, devices[j + 1].Name) > 0)
                {
                    var temp = devices[j];
                    devices[j] = devices[j + 1];
                    devices[j + 1] = temp;
                }
            }
        }
    }
}
