using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace UsbInfo.Natives.Types
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal struct USB_HUB_DESCRIPTOR
    {
        public byte bDescriptorLength;
        public byte bDescriptorType;
        public byte bNumberOfPorts;
        public short wHubCharacteristics;
        public byte bPowerOnToPowerGood;
        public byte bHubControlCurrent;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] bRemoveAndPowerMask;
    }
}