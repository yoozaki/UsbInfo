using System.Collections.Generic;
using System.Linq;
using UsbInfo.Factoryes;
using UsbInfo.Interfaces;

namespace UsbInfo.Models
{
    internal class UsbHub : IUsbHub
    {
        public UsbHub(
            int portNo,
            short vendorId,
            short productId,
            byte supportedUsbSpeed,
            short currentUsbSpeed,
            string hubDevicePath,
            IUsbNode parent)
        {
            var usbDevices = UsbDeviceEnumerator.EnumrateUsbDevices(hubDevicePath, this);

            PortNumber = usbDevices.Count;
            PortNo = portNo;
            VendorId = vendorId;
            ProductId = productId;
            SupportedUsbSpeed = supportedUsbSpeed;
            CurrentUsbSpeed = currentUsbSpeed;
            ConnectedDevices = usbDevices.Where(device => device.VendorId != 0).ToList();
            Parent = parent;
        }

        public int PortNumber { get; }
        public int PortNo { get; }
        public short VendorId { get; }
        public short ProductId { get; }
        public byte SupportedUsbSpeed { get; }
        public short CurrentUsbSpeed { get; }
        public IReadOnlyCollection<IUsbDevice> ConnectedDevices { get; }
        public IUsbNode Parent { get; }
    }
}
