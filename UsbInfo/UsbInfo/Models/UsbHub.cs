using System.Collections.Generic;
using UsbInfo.Extensions;
using UsbInfo.Factories;
using UsbInfo.Interfaces;

namespace UsbInfo.Models
{
    internal class UsbHub : IUsbHub
    {
        public int PortNumber { get; }
        public uint PortNo { get; }
        public ushort VendorId { get; }
        public ushort ProductId { get; }
        public UsbSupportSpeed SupportSpeed { get; }
        public UsbDeviceType CurrentUsbDevice { get; }
        public string DeviceDescription { get; }
        public string DeviceKey { get; }
        public string DevicePath { get; }
        public IEnumerable<IUsbDevice> ConnectedDevices { get; }
        public IUsbNode Parent { get; }

        public UsbHub(IUsbDevice usbDevice)
        {
            var childDevices = UsbDeviceListFactory.Create(usbDevice.DevicePath, this);
            PortNumber = childDevices.Count;
            ConnectedDevices = childDevices.NotOfType(typeof(UnConnectUsbDevice));

            PortNo = usbDevice.PortNo;
            VendorId = usbDevice.VendorId;
            ProductId = usbDevice.ProductId;
            SupportSpeed = usbDevice.SupportSpeed;
            CurrentUsbDevice = usbDevice.CurrentUsbDevice;
            DeviceKey = usbDevice.DeviceKey;
            DevicePath = usbDevice.DevicePath;
            DeviceDescription = usbDevice.DeviceDescription;
            Parent = usbDevice.Parent;
        }
    }
}
