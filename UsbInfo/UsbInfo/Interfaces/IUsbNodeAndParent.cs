namespace UsbInfo.Interfaces
{
    public interface IUsbNodeAndParent : IUsbNode
    {
        IUsbNodeAndParent Parent { get; }
    }
}