﻿using System.Collections.Generic;
using UsbInfo.Types;

namespace UsbInfo.Types
{
    public class UsbHub : IUsbHub
    {
        public int ProductId { get; }
        public int VendorId { get; }
        public int PortNo { get; }
        public string DevicePath { get; }
        public string DeviceName { get; }
        public int SupportedUsbSpeed { get; }
        public int CurrentUsbSpeed { get; }
        public int PortNumber { get; }
        public IEnumerable<IUsbDevice> ConnectedDevices { get; }
        public IUsbNode Parent { get; }
    }
}
