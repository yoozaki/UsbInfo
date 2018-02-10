using System;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UsbInfo.Interfaces;
using UsbInfo.Models;
using UsbInfo.Natives;
using static UsbInfo.Natives.NativeMethods;

namespace UsbInfo.Factoryes
{
    public static class UsbDeviceEnumerator
    {
        internal static IReadOnlyList<IUsbDevice> EnumrateUsbDevices(SafeFileHandle hubHandle, IUsbNode parentNode)
        {
            var nodeInformation = DeviceIoControlInvoker
                .Invoke(hubHandle, IOCTL_USB_GET_NODE_INFORMATION, new USB_NODE_INFORMATION { NodeType = 0 });

            var usbDevices = new List<IUsbDevice>();
            var portNumber = nodeInformation.HubInformation.HubDescriptor.bNumberOfPorts;
            for (int portNo = 1; portNo <= portNumber; portNo++)
            {
                var conneectInfomation = DeviceIoControlInvoker.Invoke(
                    hubHandle, IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX,
                    new USB_NODE_CONNECTION_INFORMATION_EX { ConnectionIndex = portNo });
                if (conneectInfomation.ConnectionIndex == 0)
                {
                    usbDevices.Add(UsbDevice.Empty(portNo, parentNode));
                    continue;
                }

                var isHub = Convert.ToBoolean(conneectInfomation.DeviceIsHub);
                if (isHub)
                {

                    var connectionName = DeviceIoControlInvoker.Invoke(
                        hubHandle, IOCTL_USB_GET_NODE_CONNECTION_NAME,
                        new USB_NODE_CONNECTION_NAME { ConnectionIndex = portNo });

                    usbDevices.Add(
                        new UsbHub(portNo,
                            conneectInfomation.DeviceDescriptor.idVendor,
                            conneectInfomation.DeviceDescriptor.idProduct,
                            conneectInfomation.Speed,
                            conneectInfomation.DeviceDescriptor.bcdUSB,
                            @"\\.\" + connectionName.NodeName,
                            parentNode
                        ));                    
                }
                else
                {
                    usbDevices.Add(new UsbDevice(
                        portNo,
                        conneectInfomation.DeviceDescriptor.idVendor,
                        conneectInfomation.DeviceDescriptor.idProduct,
                        conneectInfomation.Speed,
                        conneectInfomation.DeviceDescriptor.bcdUSB,
                        parentNode));
                }
            }
            return usbDevices;
        }

        internal static IReadOnlyList<IUsbDevice> EnumrateUsbDevices(string hubDevicePath, IUsbNode parentNode)
        {
            using (var hubHandle = CreateFile(
                hubDevicePath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero)) { 
                return EnumrateUsbDevices(hubHandle, parentNode);
            }
        }
    }
}