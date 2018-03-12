using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace UsbInfo.Natives.Types
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [StructLayout(LayoutKind.Sequential)]
    internal struct USB_NODE_INFORMATION
    {
        public int NodeType;
        public USB_NODE_UNION UsbNodeUnion;
    }
}