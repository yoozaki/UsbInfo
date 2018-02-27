using System.Collections.Generic;
using System.Linq;
using UsbInfo.Factories;
using UsbInfo.Interfaces;
using UsbInfo.Models;

namespace UsbInfo
{
    public static class UsbInfo
    {
        public static IEnumerable<IUsbRootHub> RootHubs()
        {
            return HostControllerFactory
                .Create()
                .Select(host => new UsbRootHub(host.RootHubPath));
        }

        public static IEnumerable<IUsbDevice> Devices()
        {
            return Devices(RootHubs().SelectMany(hub => hub.ConnectedDevices));
        }

        public static IEnumerable<IUsbDevice> Devices(ushort vid)
        {
            return Devices().Where(device => device.VendorId == vid);
        }

        public static IEnumerable<IUsbDevice> Devices(ushort vid, ushort pid)
        {
            return Devices(vid).Where(device => device.ProductId == pid);
        }

        private static IEnumerable<IUsbDevice> Devices(IEnumerable<IUsbDevice> devices)
        {
            foreach (var device in devices)
            {
                yield return device;
                foreach (var usbDevice in Devices(device.ConnectedDevices))
                {
                    yield return usbDevice;
                }
            }
        }
    }
}
