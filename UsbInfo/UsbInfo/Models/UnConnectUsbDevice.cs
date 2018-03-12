using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UsbInfo.Interfaces;

namespace UsbInfo.Models
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    internal class UnConnectUsbDevice : IUsbDevice
    {
        public IUsbNode Parent { get; }
        public IEnumerable<IUsbDevice> ConnectedDevices { get; } = new List<IUsbDevice>();
        public uint PortNo { get; }
        public ushort VendorId { get; }
        public ushort ProductId { get; }
        public UsbSupportSpeed SupportSpeed { get; }
        public UsbDeviceType CurrentUsbDevice { get; }
        public string DeviceDescription { get; }
        public string DeviceKey { get; }
        public string DevicePath { get; }
        public string SerialNumber { get; }

        public UnConnectUsbDevice(uint portNo, IUsbNode parent)
        {
            Parent = parent;
            PortNo = portNo;
        }
    }
}