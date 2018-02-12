using System;
using UsbInfo.Interfaces;
using UsbInfo.Models;
using UsbInfo.Natives;
using static UsbInfo.Natives.NativeMethods;

namespace UsbInfo.Factoryes
{
    public static class RootHubFactory
    {
        internal static IUsbRootHub Create(HostController hostController)
        {
            using (var hubHandle = CreateFile(
                hostController.RootHubPath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero))
            {
                var nodeInformation = DeviceIoControlInvoker
                    .Invoke(hubHandle, IOCTL_USB_GET_NODE_INFORMATION, new USB_NODE_INFORMATION { NodeType = 0 });

                return new UsbRootHub(
                    hostController.RootHubPath
                    nodeInformation.HubInformation.HubDescriptor.bNumberOfPorts, 
                    hubHandle);
            }
        }
    }
}
