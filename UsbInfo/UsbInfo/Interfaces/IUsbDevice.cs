namespace UsbInfo.Interfaces
{
    public interface IUsbDevice : IUsbNode
    {
        int PortNo { get; }
        short VendorId { get; }
        short ProductId { get; }
        byte SupportedUsbSpeed { get; }
        short CurrentUsbSpeed { get; }
    }
}