using System;
using UsbInfo.Natives;
using UsbInfo.Types;
using static UsbInfo.Natives.NativeMethods;

namespace UsbInfo.Factoryes
{
    public static class HostControllerFactory
    {
        public static HostController Create(string hostControllerPath)
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
