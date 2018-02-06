using System.Collections.Generic;

namespace UsbInfo.Types
{
    public interface IUsbRootNode
    {
        int PortNumber { get; }
        IEnumerable<IUsbDevice> ConnectedDevices { get; }
    }
}