using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace UsbInfo.Natives.Types
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [StructLayout(LayoutKind.Sequential)] 
    internal struct USB_DESCRIPTOR_REQUEST 
    { 
        internal uint ConnectionIndex; 
        internal USB_SETUP_PACKET SetupPacket;
        internal USB_STRING_DESCRIPTOR StringDescriptor;
    }
}