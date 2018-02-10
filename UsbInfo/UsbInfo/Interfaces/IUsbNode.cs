using System.Collections.Generic;

namespace UsbInfo.Interfaces
{
    public interface IUsbNode
    {
        IUsbNode Parent { get; }
        IReadOnlyCollection<IUsbDevice> ConnectedDevices { get; }
    }
}