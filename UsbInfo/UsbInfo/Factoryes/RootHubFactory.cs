using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbInfo.Natives;
using UsbInfo.Types;
using static UsbInfo.Natives.NativeMethods;

namespace UsbInfo.Factoryes
{
    internal static class RootHubFactory
    {
        public static void Create(HostController hostController)
        {
            using (var hubHandle = CreateFile(
                hostController.RootHubPath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero))
            {
                var nodeInformation = DeviceIoControlInvoker
                    .Invoke(hubHandle, IOCTL_USB_GET_NODE_INFORMATION, new USB_NODE_INFORMATION { NodeType = 0 });
                for (int portNo = 1; portNo < nodeInformation.HubInformation.HubDescriptor.bNumberOfPorts; portNo++)
                {
                    var connectionInformation = DeviceIoControlInvoker.Invoke(
                        hubHandle, IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX,
                        new USB_NODE_CONNECTION_INFORMATION_EX { ConnectionIndex = portNo });
                    if (connectionInformation.ConnectionIndex == 0) continue;

                    var nodeConnectionName = DeviceIoControlInvoker.Invoke(
                        hubHandle, IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME,
                        new USB_NODE_CONNECTION_DRIVERKEY_NAME() { ConnectionIndex = portNo });

                    var invoke = DeviceIoControlInvoker.Invoke(
                        hubHandle, IOCTL_USB_GET_NODE_CONNECTION_NAME,
                        new USB_NODE_CONNECTION_NAME() { ConnectionIndex = portNo });
                }
            }
        }
    }
}
