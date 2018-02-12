namespace UsbInfo.Interfaces
{
    public enum UsbSupportSpeed
    {
        LowSpeed = 0,
        FullSpeed,
        HighSpeed,
        SuperSpeed
    }

    public enum UsbDeviceType
    {
        Usb100,
        Usb110,
        Usb200,
        Usb300
    }

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