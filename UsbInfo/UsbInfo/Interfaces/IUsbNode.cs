using System.Collections.Generic;

namespace UsbInfo.Interfaces
{
    public interface IUsbNode
    {
        IUsbNode Parent { get; }
        IEnumerable<IUsbDevice> ConnectedDevices { get; }
    }
}