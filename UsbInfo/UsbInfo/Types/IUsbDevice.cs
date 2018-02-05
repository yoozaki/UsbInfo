using System.Collections.Generic;
using UsbInfo.Natives;

namespace UsbInfo.Types
{
    public interface IUsbDevice
    {
        IEnumerable<UsbDevice> Devices { get; }
        UsbHub Parent { get; }
        int ProductId { get; }
        int VendorId { get; }
        int PortNo { get; }
        string DevicePath { get; }
        string DeviceName { get; }
        int SupportedUsbSpeed { get; }
        int CurrentUsbSpeed { get; }
    }
}