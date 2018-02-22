using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace UsbInfo.Natives.Types
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal struct USB_NODE_CONNECTION_INFORMATION_EX
    {
        public uint ConnectionIndex;
        public USB_DEVICE_DESCRIPTOR DeviceDescriptor;
        public byte CurrentConfigurationValue;
        public byte Speed;
        public byte DeviceIsHub;
        public ushort DeviceAddress;
        public uint NumberOfOpenPipes;
        public uint ConnectionStatus;
    }
}