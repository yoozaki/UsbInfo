using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UsbInfo.Natives;
using UsbInfo.Types;

namespace UsbInfo
{
    public class UsbDeviceFinder
    {
        public static IEnumerable<UsbDevice> FindConnectedDevices()
        {
            return Enumerable.Empty<UsbDevice>();
        }
    }
}
