using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using UsbInfo.Interfaces;
using UsbInfo.Models;
using UsbInfo.Natives;
using UsbInfo.Natives.Types;
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

            var usbDevice = new UsbDevice(
                _portNo,
                _usbConnectInfomation.DeviceDescriptor.idVendor,
                _usbConnectInfomation.DeviceDescriptor.idProduct,
                (UsbSupportSpeed)_usbConnectInfomation.Speed,
                ConvertUsbDeviceType(_usbConnectInfomation.DeviceDescriptor.bcdUSB),
                driverKeyName,
                deviceMetaData?.DevicePath,
                deviceMetaData?.DeviceDescription,
                GetSerialNumber(),
                parentNode);

            return _usbConnectInfomation.DeviceDescriptor.bDeviceClass == 0xEF 
                        ? CreateUsbDeviceWithMiDevice(usbDevice) 
                        : usbDevice;
        }

        private string GetSerialNumber()
        {
            if (_usbConnectInfomation.DeviceDescriptor.iSerialNumber <= 0) return string.Empty;

            var request = new USB_DESCRIPTOR_REQUEST
            {
                ConnectionIndex = _portNo,
                SetupPacket =
                {
                    wValue = (short) ((USB_STRING_DESCRIPTOR_TYPE << 8) +
                                      _usbConnectInfomation.DeviceDescriptor.iSerialNumber)
                }
            };
            request.SetupPacket.wLength = (short) Marshal.SizeOf(request);
            // Language Code 
            request.SetupPacket.wIndex = 0x409; 

            var usbDescriptorRequest = DeviceIoControlInvoker.Invoke(
                _hubHandle, IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION, request);
            return usbDescriptorRequest.StringDescriptor.bString;
        }

        private static IUsbDevice CreateUsbDeviceWithMiDevice(UsbDevice usbParentDevice)
        {
            const int devicePathPrefix = 4;
            var deviceId = usbParentDevice.DevicePath
                .Substring(devicePathPrefix, usbParentDevice.DevicePath.LastIndexOf("#", StringComparison.Ordinal) - devicePathPrefix)
                .Replace("#", @"\");
            ThrowIfNotCrSuccess(CM_Locate_DevNode(out var devMiRoot, deviceId));
            ThrowIfNotCrSuccess(CM_Get_Child(out var devChild, devMiRoot));

            var miDevices = new List<UsbDevice> {CreateMiDevice(devChild, usbParentDevice)};
            while (true)
            {
                if (CM_Get_Sibling(out var devSibling, devChild) != PnpConfigrationResult.CR_SUCCESS)
                {
                    break;
                }

                miDevices.Add(CreateMiDevice(devSibling, usbParentDevice));
            }

            return new UsbDevice(usbParentDevice, miDevices);
        }

        private static UsbDevice CreateMiDevice(uint devChild, UsbDevice usbDevice)
        {
            return new UsbDevice(
                usbDevice.PortNo,
                usbDevice.VendorId,
                usbDevice.ProductId,
                usbDevice.SupportSpeed,
                usbDevice.CurrentUsbDevice,
                "",
                GetMiDeviceId(devChild),
                "",
                usbDevice.SerialNumber,
                usbDevice);
        }

        private static string GetMiDeviceId(uint devChildInst)
        {
            ThrowIfNotCrSuccess(CM_Get_Device_ID_Size(out var size, devChildInst));
            var stringBuilder = new StringBuilder(size + 1);
            ThrowIfNotCrSuccess(CM_Get_Device_ID(devChildInst, stringBuilder, stringBuilder.Capacity));
            return stringBuilder.ToString();
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

            if (bcdUsb >= 0x200 && bcdUsb < 0x300)
            {
                return UsbDeviceType.Usb200;
            }

            return UsbDeviceType.Usb300;
        }
    }
}