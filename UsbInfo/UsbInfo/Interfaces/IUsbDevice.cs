namespace UsbInfo.Interfaces
{
    public interface IUsbDevice : IUsbNode
    {
        uint PortNo { get; }
        ushort VendorId { get; }
        ushort ProductId { get; }
        UsbSupportSpeed SupportSpeed { get; }
        UsbDeviceType CurrentUsbDevice { get; }
        string DeviceDescription { get; }
        string DeviceKey { get; }
        string DevicePath { get; }
    }
}