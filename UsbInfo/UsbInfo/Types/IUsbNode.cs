namespace UsbInfo.Types
{
    public interface IUsbNode : IUsbRootNode
    {
        IUsbNode Parent { get; }
    }
}