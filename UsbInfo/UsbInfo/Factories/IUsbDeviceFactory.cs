using UsbInfo.Interfaces;

namespace UsbInfo.Factories
{
    public interface IUsbDeviceFactory
    {
        IUsbDevice Create(IUsbNode parentNode);
    }
}