using System.Collections.Generic;
using UsbInfo.Interfaces;

namespace UsbInfo.Models
{
    internal class NoConnectUsbDevice : IUsbDevice
    {
        public NoConnectUsbDevice(IUsbNode parent, int portNo)
        {
            Parent = parent;
            PortNo = portNo;
        }

        public IUsbNode Parent { get; }
        public IReadOnlyCollection<IUsbDevice> ConnectedDevices { get; } = new List<IUsbDevice>();
        public int PortNo { get; }
        public short VendorId { get; }
        public short ProductId { get; }
        public byte SupportedUsbSpeed { get; }
        public short CurrentUsbSpeed { get; }
    }

    internal class UsbDevice : IUsbDevice
    {
        public static UsbDevice Empty(int portNo, IUsbNode parentNode)
        {
            return new UsbDevice(portNo, 0, 0, 0, 0, parentNode);
        }

        public UsbDevice(int portNo, short vendorId, short productId, byte supportedUsbSpeed, short currentUsbSpeed, IUsbNode parentNode)
        {
            PortNo = portNo;
            VendorId = vendorId;
            ProductId = productId;
            SupportedUsbSpeed = supportedUsbSpeed;
            CurrentUsbSpeed = currentUsbSpeed;
            Parent = parentNode;
        }

        public int PortNo { get; }
        public short VendorId { get; }
        public short ProductId { get; }
        public byte SupportedUsbSpeed { get; }
        public short CurrentUsbSpeed { get; }
        public IUsbNode Parent { get; }

        // Not support composite device
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public IReadOnlyCollection<IUsbDevice> ConnectedDevices { get; } = new List<IUsbDevice>();
    }
}