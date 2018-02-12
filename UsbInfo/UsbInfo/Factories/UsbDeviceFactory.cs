using System;
using System.Linq;
using Microsoft.Win32.SafeHandles;
using UsbInfo.Interfaces;
using UsbInfo.Models;
using UsbInfo.Natives;
using static UsbInfo.Natives.NativeMethods;

namespace UsbInfo.Factories
{
    public class UsbDeviceFactory : IUsbDeviceFactory
    {
        private readonly uint _portNo;
        private readonly USB_NODE_CONNECTION_INFORMATION_EX _usbConnectInfomation;
        private readonly SafeFileHandle _hubHandle;
        private readonly Guid _targetDevice;

        internal UsbDeviceFactory(
            SafeFileHandle hubHandle,
            uint portNo,
            USB_NODE_CONNECTION_INFORMATION_EX conneectInfomation,
            Guid targetDevice)
        {
            _hubHandle = hubHandle;
            _portNo = portNo;
            _usbConnectInfomation = conneectInfomation;
            _targetDevice = targetDevice;
        }

        internal UsbDeviceFactory(
            SafeFileHandle hubHandle,
            uint portNo,
            USB_NODE_CONNECTION_INFORMATION_EX conneectInfomation) 
                : this(hubHandle, portNo, conneectInfomation, GUID_DEVINTERFACE_USB_DEVICE)
        {
        }

        public IUsbDevice Create(IUsbNode parentNode)
        {
            var driverKeyName = DeviceIoControlInvoker.Invoke(
                _hubHandle, IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME,
                new USB_NODE_CONNECTION_DRIVERKEY_NAME() { ConnectionIndex = _portNo }).DriverKeyName;
            var deviceMetaData = EnumerableDeviceMetaData(_targetDevice)
                .FirstOrDefault(data => data.DriverKeyName == driverKeyName);

            return new UsbDevice(
                _portNo,
                _usbConnectInfomation.DeviceDescriptor.idVendor,
                _usbConnectInfomation.DeviceDescriptor.idProduct,
                (UsbSupportSpeed)_usbConnectInfomation.Speed,
                ConvertUsbDeviceType(_usbConnectInfomation.DeviceDescriptor.bcdUSB),
                driverKeyName,
                deviceMetaData?.DevicePath,
                deviceMetaData?.DeviceDescription,
                parentNode);
        }

        private UsbDeviceType ConvertUsbDeviceType(ushort bcdUsb)
        {
            if (bcdUsb == 0x100)
            {
                return UsbDeviceType.Usb100;
            }

            if (bcdUsb == 0x110)
            {
                return UsbDeviceType.Usb110;
            }

            if (bcdUsb >= 0x200 || bcdUsb < 0x300)
            {
                return UsbDeviceType.Usb200;
            }

            return UsbDeviceType.Usb300;
        }
    }
}