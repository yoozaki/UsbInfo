using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace UsbInfo.Natives. Types
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [StructLayout(LayoutKind.Explicit)]
    internal struct USB_NODE_UNION
    {
        [FieldOffset(0)] public USB_HUB_INFORMATION HubInformation;
        [FieldOffset(0)] public USB_MI_PARENT_INFORMATION MiParentInformation;
    }

}