namespace UsbInfo.Interfaces
{
    interface IUsbHub : IUsbDevice
    {
        int PortNumber { get; }
    }
}