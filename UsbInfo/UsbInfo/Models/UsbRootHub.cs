using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using UsbInfo.Factoryes;
using UsbInfo.Interfaces;
using UsbInfo.Natives;
using static UsbInfo.Natives.NativeMethods;


namespace UsbInfo.Models
{
    // TODO Implement IUsbDevice
    internal class UsbRootHub : IUsbRootHub
    {
        // The root node is always null.
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public IUsbNode Parent { get; }

        public int PortNumber { get; }
        public IReadOnlyCollection<IUsbDevice> ConnectedDevices { get; }

        public UsbRootHub(string rootHubDevicePath)
        {
            using (var hubHandle = CreateFile(
                rootHubDevicePath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero))
            {
                var nodeInformation = DeviceIoControlInvoker
                    .Invoke(hubHandle, IOCTL_USB_GET_NODE_INFORMATION, new USB_NODE_INFORMATION { NodeType = 0 });

                PortNumber = nodeInformation.HubInformation.HubDescriptor.bNumberOfPorts;
                ConnectedDevices = UsbDeviceEnumerator.EnumrateUsbDevices(hubHandle, this)
                    .Where(device => device.VendorId != 0)
                    .ToList();
            }
        }
    }
}