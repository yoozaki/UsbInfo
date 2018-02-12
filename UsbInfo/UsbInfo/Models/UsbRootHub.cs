using System.Collections.Generic;
using UsbInfo.Extensions;
using UsbInfo.Factories;
using UsbInfo.Interfaces;

namespace UsbInfo.Models
{
    // TODO Implement IUsbDevice
    internal class UsbRootHub : IUsbRootHub
    {
        // The root node is always null.
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public IUsbNode Parent { get; }

        public int PortNumber { get; }
        public IEnumerable<IUsbDevice> ConnectedDevices { get; }

        public UsbRootHub(string rootHubDevicePath)
        {
            var usbDevices = UsbDeviceListFactory.Create(rootHubDevicePath, this);
            PortNumber = usbDevices.Count;
            ConnectedDevices = usbDevices.NotOfType(typeof(UnConnectUsbDevice));
        }
    }
}