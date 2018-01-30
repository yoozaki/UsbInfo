using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UsbInfo.Natives
{
    public class DeviceIoControlInvoker
    {
        public static T Invoke<T>(SafeFileHandle handle, int ioControlCode) where T : struct
        {
            var output = new byte[Marshal.SizeOf<T>()];
            NativeMethods.ThrowIfSetLastError(NativeMethods.DeviceIoControl(
                handle, ioControlCode, IntPtr.Zero, 0, output, output.Length, out var actualSize, IntPtr.Zero));

            using (var hGlobal = new NativeMethods.HGlobal(actualSize))
            {
                NativeMethods.ThrowIfSetLastError(NativeMethods.DeviceIoControl(
                    handle, ioControlCode, IntPtr.Zero, 0, hGlobal.Value, actualSize, out actualSize, IntPtr.Zero));
                return Marshal.PtrToStructure<T>(hGlobal.Value);
            }
        }
    }
}