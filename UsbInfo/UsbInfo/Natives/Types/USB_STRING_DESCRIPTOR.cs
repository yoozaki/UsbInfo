using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace UsbInfo.Natives.Types
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [StructLayout(LayoutKind.Sequential)] 
    internal struct USB_STRING_DESCRIPTOR
    {
        internal byte bLength;
        internal byte bDescriptorType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=255)] 
        internal string bString; 
    }
}