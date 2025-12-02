# IoT Device Monitor

## Introduction
A console-based IoT Device Monitoring System developed in C#. This application allows users to manage IoT devices (Sensors, Gateways), track their status, and persist data using JSON.

## Features
- **Add Devices**: Support for Sensors and Gateways.
- **Update Status**: Mark devices as Online, Offline, or Maintenance.
- **Search**: Find devices by name.
- **Sort**: Sort devices by name using Bubble Sort.
- **Persistence**: Data is saved to `devices.json`.
- **Logging**: Key events are logged to `log.txt`.
- **Reports**: Generate a simple system health report.

## How to Run
1. Ensure .NET 8.0 SDK (or later) is installed.
2. Navigate to the solution directory.
3. Run the application:
   ```bash
   dotnet run --project IoTMonitor.ConsoleApp
   ```
4. Run tests:
   ```bash
   dotnet test
   ```

## Design Choices

### Object-Oriented Programming
- **Inheritance**: `Device` is the abstract base class. `Sensor` and `Gateway` inherit from it.
- **Polymorphism**: `ToString()` is overridden to provide specific details. `Type` property is overridden.
- **Encapsulation**: Fields are private, properties are public. Logic is encapsulated in `DeviceService`.

### Design Patterns
- **Singleton**: Used for `Logger` to ensure a single instance handles file writing.
- **Repository**: `DeviceRepository` handles data access and persistence, decoupling storage from business logic.
- **Strategy**: `ISortStrategy` allows swapping sorting algorithms. `BubbleSortStrategy` is implemented.

### Algorithms
- **Search**: Linear Search is used to find devices by name.
- **Sort**: Bubble Sort is manually implemented to demonstrate algorithm knowledge.

### Data Persistence
- Uses `System.Text.Json` for serialization.
- Polymorphic serialization is handled using `[JsonDerivedType]` attributes.

## Testing
- Unit tests are implemented using xUnit.
- Covers adding devices, duplicate prevention, status updates, search, and sorting.
