using System;
using System.Runtime.InteropServices;

namespace UsbInfo.Natives
{
    public partial class NativeMethods
    {
        public class HGlobal : IDisposable
        {
            [DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
            static extern void ZeroMemory(IntPtr dest, IntPtr size);

            public IntPtr Value { get; }

            public HGlobal(int size)
            {
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