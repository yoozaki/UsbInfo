using System.Collections.Generic;
using System.Linq;
using UsbInfo.Factoryes;
using UsbInfo.Interfaces;
using UsbInfo.Models;

namespace UsbInfo
{
    public class UsbInfo
    {
        public static IEnumerable<IUsbRootHub> RootHubs()
        {
            return HostControllerEnumerator
                .Enumerable()
                .Select(host => new UsbRootHub(host.RootHubPath));
        }

        public static IEnumerable<IUsbDevice> Devices()
        {
            return RootHubs().SelectMany(hub => hub.ConnectedDevices);
        }

        public static IEnumerable<IUsbDevice> Devices(short vid, short pid)
        {
            return Devices().Where(device => device.VendorId == vid && device.ProductId == pid);
        }
    }
}
