﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UsbInfo.Natives
{
    public class DeviceIoControlInvoker
    {
        public static T Invoke<T>(SafeFileHandle handle, int ioControlCode) where T : struct
        {
            using (var output = new NativeMethods.HGlobal(GetActualSize<T>(handle, ioControlCode)))
            {
                NativeMethods.ThrowIfSetLastError(NativeMethods.DeviceIoControl(
                    handle, ioControlCode, IntPtr.Zero, 0, output.Value, output.Size, out var _, IntPtr.Zero));
                return Marshal.PtrToStructure<T>(output.Value);
            }
        }

        public static T Invoke<T>(SafeFileHandle handle, int ioControlCode, T inputObject) where T : struct
        {
            using (var input = new NativeMethods.HGlobal<T>(inputObject))
            using (var output = new NativeMethods.HGlobal(GetActualSize<T>(handle, ioControlCode)))
            {
                NativeMethods.ThrowIfSetLastError(NativeMethods.DeviceIoControl(
                    handle, ioControlCode, input.Value, input.Size, output.Value, output.Size, out var _, IntPtr.Zero));
                return Marshal.PtrToStructure<T>(output.Value);
            }
        }

        private static int GetActualSize<T>(SafeFileHandle handle, int ioControlCode) where T : struct
        {
            var output = new byte[Marshal.SizeOf<T>()];
            NativeMethods.ThrowIfSetLastError(NativeMethods.DeviceIoControl(
                handle, ioControlCode, IntPtr.Zero, 0, output, output.Length, out var actualSize, IntPtr.Zero));
            return actualSize;
        }
    }
}