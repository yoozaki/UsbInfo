﻿using System;
using System.Collections.Generic;
using System.Linq;
using UsbInfo.Extensions;
using UsbInfo.Models;
using UsbInfo.Natives;
using UsbInfo.Natives.Types;

namespace UsbInfo.Factories
{
    public static class HostControllerFactory
    {
        internal static IEnumerable<HostController> Create()
        {
            return NativeMethods.HostControllerPaths().Select(Create);
        }

        private static HostController Create(string hostControllerPath)
        {
            using (var hostHanle = NativeMethods.CreateFile(
                hostControllerPath, NativeMethods.GENERIC_WRITE, NativeMethods.FILE_SHARE_WRITE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0, IntPtr.Zero))
            {
                NativeMethods.ThrowIfSetLastError(!hostHanle.IsInvalid);
                var usbRootHubName = DeviceIoControlInvoker.Invoke<USB_ROOT_HUB_NAME>(hostHanle, NativeMethods.IOCTL_USB_GET_ROOT_HUB_NAME);

                return new HostController(hostControllerPath, usbRootHubName.DevicePath());
            }
        }
    }
}
