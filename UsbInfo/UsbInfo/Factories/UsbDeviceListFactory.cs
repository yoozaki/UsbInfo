using System;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UsbInfo.Interfaces;
using UsbInfo.Natives;

namespace UsbInfo.Factories
{
    public static class UsbDeviceListFactory
    {
        internal static IReadOnlyList<IUsbDevice> Create(string hubDevicePath, IUsbNode parentNode)
        {
            using (var hubHandle = NativeMethods.CreateFile(
                hubDevicePath, NativeMethods.GENERIC_WRITE, NativeMethods.FILE_SHARE_WRITE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero)) { 
                return Create(hubHandle, parentNode);
            }
        }

        private static IReadOnlyList<IUsbDevice> Create(SafeFileHandle hubHandle, IUsbNode parentNode)
        {
            var nodeInformation = DeviceIoControlInvoker
                .Invoke(hubHandle, NativeMethods.IOCTL_USB_GET_NODE_INFORMATION, new NativeMethods.USB_NODE_INFORMATION { NodeType = 0 });

            var usbDevices = new List<IUsbDevice>();
            var portNumber = nodeInformation.HubInformation.HubDescriptor.bNumberOfPorts;
            for (uint portNo = 1; portNo <= portNumber; portNo++)
            {
                var usbDeviceFactory = AbstractUsbDeviceFactory.CreateFactory(hubHandle, portNo);
                usbDevices.Add(usbDeviceFactory.Create(parentNode));
            }
            return usbDevices;
        }
    }
}