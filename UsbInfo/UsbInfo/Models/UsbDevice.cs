using System.Collections.Generic;
using UsbInfo.Interfaces;

namespace UsbInfo.Models
{
    internal class UsbDevice : IUsbDevice
    {
        public uint PortNo { get; }
        public ushort VendorId { get; }
        public ushort ProductId { get; }
        public UsbSupportSpeed SupportSpeed { get; }
        public UsbDeviceType CurrentUsbDevice { get; }
        public string DeviceDescription { get; }
        public string DeviceKey { get; }
        public string DevicePath { get; }
        public string SerialNumber { get; }
        public IUsbNode Parent { get; }
        public IEnumerable<IUsbDevice> ConnectedDevices { get; } = new List<IUsbDevice>();

        public UsbDevice(
            uint portNo,
            ushort vendorId,
            ushort productId,
            UsbSupportSpeed supportSpeed,
            UsbDeviceType currentUsbDevice,
            string deviceKey,
            string devicePath,
            string fridlyName,
            string serialNumber,
            IUsbNode parentNode)
        {
            PortNo = portNo;
            VendorId = vendorId;
            ProductId = productId;
            SupportSpeed = supportSpeed;
            CurrentUsbDevice = currentUsbDevice;
            Parent = parentNode;
            DeviceKey = deviceKey;
            DevicePath = devicePath;
            DeviceDescription = fridlyName;
            SerialNumber = serialNumber;
        }

        public UsbDevice(
            UsbDevice usbDevice,
            IEnumerable<UsbDevice> connectedDevices)
            : this(
                usbDevice.PortNo,
                usbDevice.VendorId,
                usbDevice.ProductId,
                usbDevice.SupportSpeed,
                usbDevice.CurrentUsbDevice,
                usbDevice.DeviceKey,
                usbDevice.DevicePath,
                usbDevice.DeviceDescription,
                usbDevice.SerialNumber,
                usbDevice.Parent)
        {
            ConnectedDevices = connectedDevices;
        }
    }
}