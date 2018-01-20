using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
