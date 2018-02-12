using UsbInfo.Interfaces;
using UsbInfo.Models;

namespace UsbInfo.Factories
{
    internal class UnConnectUsbDeviceFactory : IUsbDeviceFactory
    {
        private readonly uint _portNo;

        public UnConnectUsbDeviceFactory(uint portNo)
        {
            _portNo = portNo;
        }

        public IUsbDevice Create(IUsbNode parentNode)
        {
            return new UnConnectUsbDevice(_portNo, parentNode);
        }
    }
}