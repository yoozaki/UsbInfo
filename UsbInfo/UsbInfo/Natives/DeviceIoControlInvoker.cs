using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UsbInfo.Natives
{
    internal class DeviceIoControlInvoker
    {
        internal static T Invoke<T>(SafeFileHandle handle, int ioControlCode) where T : struct
        {
            using (var output = new HGlobal(GetActualSize<T>(handle, ioControlCode)))
            {
                NativeMethods.ThrowIfSetLastError(NativeMethods.DeviceIoControl(
                    handle, ioControlCode, IntPtr.Zero, 0, output.Value, output.Size, out var _, IntPtr.Zero));
                return Marshal.PtrToStructure<T>(output.Value);
            }
        }

        internal static T Invoke<T>(SafeFileHandle handle, int ioControlCode, T inputAndOutputStruct) where T : struct
        {
            using (var inputAndOutputPtr = new HGlobal<T>(inputAndOutputStruct))
            {
                NativeMethods.ThrowIfSetLastError(NativeMethods.DeviceIoControl(
                    handle, ioControlCode, inputAndOutputPtr.Value, inputAndOutputPtr.Size, 
                    inputAndOutputPtr.Value, inputAndOutputPtr.Size, out var _, IntPtr.Zero));
                return Marshal.PtrToStructure<T>(inputAndOutputPtr.Value);
            }
        }

        private static int GetActualSize<T>(SafeFileHandle handle, int ioControlCode) where T : struct
        {
            var output = new byte[Marshal.SizeOf<T>()];
            var gcHandle = GCHandle.Alloc(output, GCHandleType.Pinned);
            using (Disposable.Create(() => gcHandle.Free()))
            {
                NativeMethods.ThrowIfSetLastError(NativeMethods.DeviceIoControl(
                    handle, ioControlCode, IntPtr.Zero, 0, output, output.Length, out var actualSize, IntPtr.Zero));
                return actualSize;
            }
        }
    }
}