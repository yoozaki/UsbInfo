using UsbInfo.Natives;

namespace UsbInfo.Extensions
{
    // 
    internal static class UsbRootHubNameExtensions
    {
        internal static string DevicePath(this NativeMethods.USB_ROOT_HUB_NAME rootHubName)
        {
            return @"\\.\" + rootHubName.RootHubName;
        }
    }
}
