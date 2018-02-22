using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace UsbInfo.Natives.Types
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal struct USB_NODE_CONNECTION_DRIVERKEY_NAME
    {
        public uint ConnectionIndex;
        public readonly uint ActualLength;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeMethods.BUFFER_SIZE)]
        public readonly string DriverKeyName;
    }
}