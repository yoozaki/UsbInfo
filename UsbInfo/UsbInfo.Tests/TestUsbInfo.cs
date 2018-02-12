using Xunit;

namespace UsbInfo.Tests
{
    public class TestUsbInfo
    {
        [Fact]
        public void TestRootHub()
        {
            var usbRootHub = UsbInfo.Devices();
            Assert.NotEmpty(usbRootHub);
        }

        [Fact]
        public void TestDeviceFromVidPid()
        {
            var usbRootHub = UsbInfo.Devices(0x05E3, 0x0608);
            Assert.NotEmpty(usbRootHub);
        }
    }
}
