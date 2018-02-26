using System;
using Microsoft.Win32.SafeHandles;
using UsbInfo.Natives;
using UsbInfo.Natives.Types;

namespace UsbInfo.Factories
{
    internal static class AbstractUsbDeviceFactory
    {
        internal static IUsbDeviceFactory CreateFactory(SafeFileHandle hubHandle, uint portNo)
        {
            var conneectInfomation = DeviceIoControlInvoker.Invoke(
                hubHandle, NativeMethods.IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX,
                new USB_NODE_CONNECTION_INFORMATION_EX { ConnectionIndex = portNo });

            if (conneectInfomation.ConnectionStatus != 1)
            {
                return new UnConnectUsbDeviceFactory(portNo);
            }

            if (Convert.ToBoolean(conneectInfomation.DeviceIsHub))
            {
                return new UsbHubFactory(hubHandle, portNo, conneectInfomation);
            }

            return new UsbDeviceFactory(hubHandle, portNo, conneectInfomation);
        }
    }
}