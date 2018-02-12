using System;
using Microsoft.Win32.SafeHandles;
using UsbInfo.Natives;

namespace UsbInfo.Factories
{
    internal static class AbstractUsbDeviceFactory
    {
        internal static IUsbDeviceFactory CreateFactory(SafeFileHandle hubHandle, uint portNo)
        {
            var conneectInfomation = DeviceIoControlInvoker.Invoke(
                hubHandle, NativeMethods.IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX,
                new NativeMethods.USB_NODE_CONNECTION_INFORMATION_EX { ConnectionIndex = portNo });

            if (conneectInfomation.ConnectionIndex == 0)
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