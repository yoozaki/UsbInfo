using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace UsbInfo.Natives.Types
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal struct USB_HUB_INFORMATION
    {
        public USB_HUB_DESCRIPTOR HubDescriptor;
        public readonly byte HubIsBusPowered;
    }
}