using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UsbInfo.Natives
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    public class NativeMethods
    {
        public const int IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX = 0x220448;
        public const int IOCTL_USB_GET_NODE_INFORMATION = 0x220408;
        public const int IOCTL_USB_GET_ROOT_HUB_NAME = 0x220408;
        public const int IOCTL_USB_GET_NODE_CONNECTION_NAME = 0x220414;
        public const int IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME = 0x220420;

        // see https://docs.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-device
        public static string GUID_DEVINTERFACE_USB_DEVICE = "A5DCBF10-6530-11D2-901F-00C04FB951ED";

        // see https://docs.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-host-controller
        public static string GUID_DEVINTERFACE_USB_HOST_CONTROLLER = "3ABF6F2D-71C4-462A-8A92-1E6861E6AF27";

        // we use registry value. see https://msdn.microsoft.com/en-us/library/windows/desktop/ms724872(v=vs.85).aspx
        const int BUFFER_SIZE = 2048;

        public const int DIGCF_PRESENT = 0x00000002;
        public const int DIGCF_DEVICEINTERFACE = 0x00000010;

        public const int GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int OPEN_EXISTING = 0x3;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct USB_DEVICE_DESCRIPTOR
        {
            public byte bLength;
            public byte bDescriptorType;
            public short bcdUSB;
            public byte bDeviceClass;
            public byte bDeviceSubClass;
            public byte bDeviceProtocol;
            public byte bMaxPacketSize0;
            public short idVendor;
            public short idProduct;
            public short bcdDevice;
            public byte iManufacturer;
            public byte iProduct;
            public byte iSerialNumber;
            public byte bNumConfigurations;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct USB_NODE_CONNECTION_INFORMATION_EX
        {
            public int ConnectionIndex;
            public USB_DEVICE_DESCRIPTOR DeviceDescriptor;
            public byte CurrentConfigurationValue;
            public byte Speed;
            public byte DeviceIsHub;
            public short DeviceAddress;
            public int NumberOfOpenPipes;
            public int ConnectionStatus;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public int DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DevicePath;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct USB_NODE_CONNECTION_NAME
        {
            public int ConnectionIndex;
            public int ActualLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string NodeName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct USB_NODE_CONNECTION_DRIVERKEY_NAME
        {
            public int ConnectionIndex;
            public int ActualLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DriverKeyName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct USB_ROOT_HUB_NAME
        {
            public int ActualLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string RootHubName;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct USB_NODE_INFORMATION
        {
            public int NodeType;
            public USB_HUB_INFORMATION HubInformation;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct USB_HUB_INFORMATION
        {
            public USB_HUB_DESCRIPTOR HubDescriptor;
            public byte HubIsBusPowered;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct USB_HUB_DESCRIPTOR
        {
            public byte bDescriptorLength;
            public byte bDescriptorType;
            public byte bNumberOfPorts;
            public short wHubCharacteristics;
            public byte bPowerOnToPowerGood;
            public byte bHubControlCurrent;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] bRemoveAndPowerMask;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string lpFileName,
            int dwDesiredAccess,
            int dwShareMode,
            IntPtr lpSecurityAttributes,
            int dwCreationDisposition,
            int dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            int dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            byte[] lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            int dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetupDiOpenDeviceInfo(
            SafeDeviceHandle DeviceInfoSet,
            string DeviceInstanceId,
            IntPtr hwndParent,
            int OpenFlags,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern SafeDeviceHandle SetupDiGetClassDevs(
            ref Guid classGuid,
            IntPtr enumerator,
            IntPtr hwndParent,
            int flags
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern SafeDeviceHandle SetupDiGetClassDevs(
            IntPtr classGuid,
            IntPtr enumerator,
            IntPtr hwndParent,
            int flags
        );

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiDestroyDeviceInfoList(
            IntPtr deviceInfoSet
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetupDiEnumDeviceInfo(
            SafeDeviceHandle DeviceInfoSet,
            int MemberIndex,
            ref SP_DEVINFO_DATA deviceData
        );

        [DllImport("setupapi.dll")]
        public static extern bool SetupDiEnumDeviceInterfaces(
            SafeDeviceHandle DeviceInfoSet,
            IntPtr DeviceInfoData, 
            ref Guid InterfaceClassGuid,
            int MemberIndex,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            IntPtr DeviceInterfaceDetailData,
            int DeviceInterfaceDetailDataSize,
            ref int RequiredSize,
            IntPtr DeviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData,
            int DeviceInterfaceDetailDataSize,
            ref int RequiredSize,
            IntPtr DeviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData,
            int DeviceInterfaceDetailDataSize,
            ref int RequiredSize, SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetupDiGetDeviceRegistryProperty(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            int iProperty,
            ref int PropertyRegDataType,
            byte[] PropertyBuffer,
            int PropertyBufferSize,
            ref int RequiredSize
        );


        public static string GetRootHub(string hostController)
        {
            using (var hostHanle = CreateFile(
                hostController, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero))
            {
                ThrowIfSetLastError(!hostHanle.IsInvalid);

                var usbRootHubName = DeviceIoControlInvoker.Invoke<USB_ROOT_HUB_NAME>(hostHanle, IOCTL_USB_GET_ROOT_HUB_NAME);
                var rootHubName = @"\\.\" + usbRootHubName.RootHubName;
                using (var hubHandle = CreateFile(
                    rootHubName, GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero)) {
                    var nodeInformation = DeviceIoControlInvoker
                        .Invoke(hubHandle, IOCTL_USB_GET_NODE_INFORMATION, new USB_NODE_INFORMATION {NodeType = 0});

                    for (int portNo = 1; portNo < nodeInformation.HubInformation.HubDescriptor.bNumberOfPorts; portNo++)
                    {
                        var deviceIoControl = DeviceIoControlInvoker.Invoke(
                            hubHandle, IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX,
                            new USB_NODE_CONNECTION_INFORMATION_EX {ConnectionIndex = portNo});
                        if (deviceIoControl.ConnectionIndex ==0) continue;

                        var nodeConnectionName = DeviceIoControlInvoker.Invoke(
                            hubHandle, IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME,
                            new USB_NODE_CONNECTION_DRIVERKEY_NAME() {ConnectionIndex = portNo});

                        var invoke = DeviceIoControlInvoker.Invoke(
                            hubHandle, IOCTL_USB_GET_NODE_CONNECTION_NAME,
                            new USB_NODE_CONNECTION_NAME() { ConnectionIndex = portNo });
                    }
                }

                return rootHubName;
            }
        }

        public static void ThrowIfSetLastError(bool result)
        {
            if (!result)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public static IEnumerable<SP_DEVINFO_DATA> HostControllers()
        {
            var guid = new Guid(GUID_DEVINTERFACE_USB_HOST_CONTROLLER);
            using (var hostHandles = SetupDiGetClassDevs(
                ref guid, IntPtr.Zero, IntPtr.Zero,
                DIGCF_PRESENT | DIGCF_DEVICEINTERFACE))
            {
                var deviceInfo = new SP_DEVINFO_DATA();
                deviceInfo.cbSize = Marshal.SizeOf(deviceInfo);
                for (int deviceInfoIndex = 0; 
                    SetupDiEnumDeviceInfo(
                        hostHandles, deviceInfoIndex, ref deviceInfo);
                    deviceInfoIndex++)
                {
                    yield return deviceInfo;
                }
            }
        }

        public static IEnumerable<string> HostControllerPaths()
        {
            var classGuid = new Guid(GUID_DEVINTERFACE_USB_HOST_CONTROLLER);
            foreach (var devicePath in EnumerableDevicePaths(classGuid))
            {
                yield return devicePath;
            }
        }

        private static IEnumerable<string> EnumerableDevicePaths(Guid classGuid)
        {
            using (var hostHandles = SetupDiGetClassDevs(
                ref classGuid,
                IntPtr.Zero,
                IntPtr.Zero,
                DIGCF_PRESENT | DIGCF_DEVICEINTERFACE))
            {
                var deviceInstace = new SP_DEVICE_INTERFACE_DATA();
                deviceInstace.cbSize = Marshal.SizeOf(deviceInstace);

                for (int deviceIndex = 0;
                    SetupDiEnumDeviceInterfaces(
                        hostHandles, IntPtr.Zero, ref classGuid,
                        deviceIndex, ref deviceInstace);
                    deviceIndex++)
                {
                    var deviceInterfaceDetailDataSize = 
                        GetDeviceInterfaceDetailDataSize(hostHandles, ref deviceInstace);
                    var deviceInstaceDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA
                    {
                        cbSize = IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8
                    };
                    if (SetupDiGetDeviceInterfaceDetail(
                        hostHandles,
                        ref deviceInstace,
                        ref deviceInstaceDetail,
                        deviceInterfaceDetailDataSize,
                        ref deviceInterfaceDetailDataSize,
                        IntPtr.Zero))
                    {
                        yield return deviceInstaceDetail.DevicePath;
                    }
                }
            }
        }

        private static int GetDeviceInterfaceDetailDataSize(SafeDeviceHandle hostHandles,
            ref SP_DEVICE_INTERFACE_DATA deviceInstace)
        {
            int deviceInterfaceDetailDataSize = 0;
            SetupDiGetDeviceInterfaceDetail(
                hostHandles,
                ref deviceInstace,
                IntPtr.Zero,
                0,
                ref deviceInterfaceDetailDataSize,
                IntPtr.Zero);

            return deviceInterfaceDetailDataSize;
        }
    }
}