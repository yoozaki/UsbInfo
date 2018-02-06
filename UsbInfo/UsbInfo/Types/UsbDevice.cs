namespace UsbInfo.Types
{
    public class UsbDevice : IUsbDevice
    {
        public int ProductId { get; }
        public int VendorId { get; }
        public int PortNo { get; }
        public string DevicePath { get; }
        public string DeviceName { get; }
        public int SupportedUsbSpeed { get; }
        public int CurrentUsbSpeed { get; }
    }
}