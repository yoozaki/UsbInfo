using System;
using System.Runtime.InteropServices;

namespace UsbInfo.Natives
{
    internal class HGlobal<T> : IDisposable where T : struct
    {
        public int Size { get; }
        public IntPtr Value { get; }

        public HGlobal(T obj)
        {
            Size = Marshal.SizeOf<T>();
            Value = Marshal.AllocHGlobal(Size);
            Marshal.StructureToPtr(obj, Value, true);
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(Value);
        }
    }

    internal class HGlobal : IDisposable
    {
        [DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
        static extern void ZeroMemory(IntPtr dest, IntPtr size);

        public int Size { get; }
        public IntPtr Value { get; }

        public HGlobal(int size)
        {
            Size = size;
            Value = Marshal.AllocHGlobal(size);
            ZeroMemory(Value, new IntPtr(size));
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(Value);
        }
    }
}