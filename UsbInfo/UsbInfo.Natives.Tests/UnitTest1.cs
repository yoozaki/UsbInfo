using System;
using System.Linq;
using Xunit;

namespace UsbInfo.Natives.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestHostControllers()
        {
            var spDevinfoDatas = NativeMethods.HostControllers();
            Assert.NotEmpty(spDevinfoDatas);
        }

        [Fact]
        public void TestHostControllerPaths()
        {
            var spDevinfoDatas = NativeMethods.HostControllerPaths();
            Assert.NotEmpty(spDevinfoDatas);
        }

        [Fact]
        public void TestRootHub()
        {
            var spDevinfoDatas = NativeMethods.HostControllerPaths();
            var rootHub = NativeMethods.GetRootHub(spDevinfoDatas.First());
            Assert.NotEmpty(spDevinfoDatas);
        }
    }
}
