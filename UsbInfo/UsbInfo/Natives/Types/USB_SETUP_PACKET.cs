using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace UsbInfo.Natives.Types
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [StructLayout(LayoutKind.Sequential)] 
    internal struct USB_SETUP_PACKET 
    { 
        internal byte bmRequest; 
        internal byte bRequest; 
        internal short wValue; 
        internal short wIndex; 
        internal short wLength; 
    }
}