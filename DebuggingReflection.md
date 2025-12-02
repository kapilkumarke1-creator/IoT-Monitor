# Debugging Reflection

## Bug Description
**Issue:** During the implementation of data persistence, I encountered a `System.NotSupportedException` when trying to deserialize the `devices.json` file. The error message stated that deserialization of the abstract type `Device` was not supported.

**Investigation:**
1.  I placed a breakpoint in the `DeviceRepository.LoadDevices()` method at the `JsonSerializer.Deserialize` line.
2.  I inspected the `json` string variable and confirmed it contained valid JSON data representing both `Sensor` and `Gateway` objects.
3.  I checked the `Device` class definition and confirmed it was marked as `abstract`.
4.  I realized that `System.Text.Json` does not automatically know which derived class to instantiate when it encounters a JSON object for an abstract base class.

**Fix:**
I researched how to handle polymorphism in `System.Text.Json` and found that I needed to use the `[JsonDerivedType]` attribute.
I modified the `Device` class to include these attributes, specifying a type discriminator for each subclass:

```csharp
[JsonDerivedType(typeof(Sensor), typeDiscriminator: "Sensor")]
[JsonDerivedType(typeof(Gateway), typeDiscriminator: "Gateway")]
public abstract class Device { ... }
```

**Verification:**
After applying the fix, I re-ran the application and the unit tests. The `LoadDevices` method successfully deserialized the list, correctly instantiating `Sensor` and `Gateway` objects based on the discriminator. The `AddDevice_ShouldAddDevice_WhenDeviceIsNew` test passed, confirming persistence was working correctly.
