using System;
using System.Runtime.InteropServices;

namespace UsbInfo.Natives
{
    public partial class NativeMethods
    {
        public class HGlobal<T> : IDisposable where T : struct 
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

        public class HGlobal : IDisposable
        {
            [DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
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
}