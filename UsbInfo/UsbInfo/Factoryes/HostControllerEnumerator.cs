using System;
using System.Collections.Generic;
using System.Linq;
using UsbInfo.Models;
using UsbInfo.Natives;
using static UsbInfo.Natives.NativeMethods;

namespace UsbInfo.Factoryes
{
    public class HostControllerEnumerator
    {
        internal static IEnumerable<HostController> Enumerable()
        {
            return HostControllerPaths().Select(CreateHostController);
        }

        private static HostController CreateHostController(string hostControllerPath)
        {
            using (var hostHanle = CreateFile(
                hostControllerPath, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero))
            {
                ThrowIfSetLastError(!hostHanle.IsInvalid);
                var usbRootHubName = DeviceIoControlInvoker.Invoke<USB_ROOT_HUB_NAME>(hostHanle, IOCTL_USB_GET_ROOT_HUB_NAME);

                return new HostController(hostControllerPath, usbRootHubName.RootHubName);
            }
        }
    }
}
