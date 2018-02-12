using System.Collections.Generic;
using UsbInfo.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace UsbInfo.Tests
{
    public class TestUsbInfo
    {
        private readonly ITestOutputHelper _output;
        public TestUsbInfo(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestDumpDevice()
        {
            var usbRootHub = UsbInfo.RootHubs();
            foreach (var rootHub in usbRootHub)
            {
                _output.WriteLine("RootHub");
                DumpDevice(rootHub.ConnectedDevices, "\t");
            }
        }

        private void DumpDevice(IEnumerable<IUsbDevice> devices, string indent)
        {
            foreach (var usbDevice in devices)
            {
                _output.WriteLine($"{indent}DeviceDescription:{usbDevice.DeviceDescription}");
                _output.WriteLine($"{indent}PortNo:{usbDevice.PortNo}");
                _output.WriteLine($"{indent}VendorId:0x{usbDevice.VendorId:X}");
                _output.WriteLine($"{indent}ProductId:0x{usbDevice.ProductId:X}");
                _output.WriteLine($"{indent}SupportSpeed:{usbDevice.SupportSpeed}");
                _output.WriteLine($"{indent}CurrentUsbDevice:{usbDevice.CurrentUsbDevice}");
                _output.WriteLine($"{indent}DeviceKey:{usbDevice.DeviceKey}");
                _output.WriteLine($"{indent}DevicePath:{usbDevice.DevicePath}");
                _output.WriteLine("");
                DumpDevice(usbDevice.ConnectedDevices, indent + "\t");
            }
        }

        [Fact]
        public void TestRootHub()
        {
            var usbDevices = UsbInfo.Devices();
            Assert.NotEmpty(usbDevices);
        }

        [Fact]
        public void TestDeviceFromVidPid()
        {
            var usbDevices = UsbInfo.Devices(0x05E3, 0x0608);
            Assert.NotEmpty(usbDevices);
        }
    }
}
