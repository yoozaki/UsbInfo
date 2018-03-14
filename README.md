UsbInfo
===
This library can get USB device information in .NET.

[![Nuget](http://img.shields.io/nuget/v/UsbInfo.svg?style=flat)](https://www.nuget.org/packages/UsbInfo/)

Overview
---
The UsbInfo class can find a list of IUsbDevice class by specifying the vendor ID, product ID or by scanning the root hub list.

**Supprots**
- USB3.0 device
- Serial number
- PortNo
- Device name 

Sample
---
Scanned dump from USB root hub.
```csharp
[Fact]
public void TestDumpDevice()
{
    var usbRootHub = UsbInfo.RootHubs();
    foreach (var rootHub in usbRootHub)
    {
        _output.WriteLine("RootHub");
        DumpDevice(rootHub.ConnectedDevices);
    }
}

private void DumpDevice(IEnumerable<IUsbDevice> devices)
{
    foreach (var usbDevice in devices)
    {
        var deviceDescription = usbDevice.DeviceDescription;
        var portNo = usbDevice.PortNo;
        var vendorId = usbDevice.VendorId;
        var productId = usbDevice.ProductId;
        var supportSpeed = usbDevice.SupportSpeed;
        var currentUsbDevice = usbDevice.CurrentUsbDevice;
        var deviceKey = usbDevice.DeviceKey;
        var devicePath = usbDevice.DevicePath;
        var serialNumber = usbDevice.SerialNumber;
        DumpDevice(usbDevice.ConnectedDevices);
    }
}
```

NuGet
---
[UsbInfo](https://www.nuget.org/packages/UsbInfo)

License
---
This library is under the MIT License.