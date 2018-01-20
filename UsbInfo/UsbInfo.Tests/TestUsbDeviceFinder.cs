using System;
using System.Linq;
using Xunit;

namespace UsbInfo.Tests
{
    public class TestUsbDeviceFinder
    {
        [Fact]
        public void TestFindConnectedDevices()
        {
            Assert.Empty(UsbDeviceFinder.FindConnectedDevices());
        }
    }
}
