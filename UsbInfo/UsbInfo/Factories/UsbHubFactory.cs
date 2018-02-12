using System;
using Microsoft.Win32.SafeHandles;
using UsbInfo.Extensions;
using UsbInfo.Interfaces;
using UsbInfo.Models;
using UsbInfo.Natives;
using static UsbInfo.Natives.NativeMethods;

namespace UsbInfo.Factories
{
    internal class UsbHubFactory : IUsbDeviceFactory
    {
        private readonly SafeFileHandle _hubHandle;
        private readonly uint _portNo;
        private readonly USB_NODE_CONNECTION_INFORMATION_EX _usbConnectInfomation;

        public UsbHubFactory(SafeFileHandle hubHandle, uint portNo, USB_NODE_CONNECTION_INFORMATION_EX conneectInfomation)
        {
            _portNo = portNo;
            _hubHandle = hubHandle;
            _usbConnectInfomation = conneectInfomation;
        }

        public IUsbDevice Create(IUsbNode parentNode)
        {
            var usbDeviceFactory = new UsbDeviceFactory(_hubHandle, _portNo, _usbConnectInfomation, GUID_DEVINTERFACE_USB_HUB);
            return new UsbHub(usbDeviceFactory.Create(parentNode));
        }
    }
}