namespace UsbInfo
{
    public class UsbDevice
    {
        public short VenderId { get; }
        public short ProductId { get; }

        public UsbDevice(short venderId, short productId)
        {
            ProductId = productId;
            VenderId = venderId;
        }
    }
}