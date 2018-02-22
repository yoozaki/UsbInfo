using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace UsbInfo.Natives.Types
{
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal struct USB_NODE_INFORMATION
    {
        public int NodeType;
        public USB_NODE_UNION UsbNodeUnion;
    }
}