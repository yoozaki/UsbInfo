using Xunit;

namespace UsbInfo.Tests
{
    public class TestUsbInfo
    {
        [Fact]
        public void TestRootHub()
        {
            var usbRootHub = UsbInfo.RootHubs();
            Assert.NotEmpty(usbRootHub);
        }
    }
}
