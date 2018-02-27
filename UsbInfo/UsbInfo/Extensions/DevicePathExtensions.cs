using UsbInfo.Natives.Types;

namespace UsbInfo.Extensions
{
    // 
    internal static class UsbRootHubNameExtensions
    {
        internal static string DevicePath(this USB_ROOT_HUB_NAME rootHubName)
        {
            return @"\\.\" + rootHubName.RootHubName;
        }
    }
}
