using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace UsbInfo.Natives
{
    public class SafeDeviceHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeDeviceHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle() 
            => NativeMethods.SetupDiDestroyDeviceInfoList(handle);
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    public class NativeMethods
    {
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


        internal const short HIDP_INPUT = 0;
        internal const short HIDP_OUTPUT = 1;

        internal const short HIDP_FEATURE = 2;
        [StructLayout(LayoutKind.Sequential)]
        internal struct HIDD_ATTRIBUTES
        {
            internal int Size;
            internal ushort VendorID;
            internal ushort ProductID;
            internal short VersionNumber;
        }

        [DllImport("hid.dll")]
        static internal extern bool HidD_GetAttributes(IntPtr hidDeviceObject, ref HIDD_ATTRIBUTES attributes);

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
            //public IntPtr PipeList; 
        }

        [DllImport("setupapi.dll")]
        public static extern int CM_Get_Parent(
            out UInt32 pdnDevInst,
            int dnDevInst,
            int ulFlags
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetupDiOpenDeviceInfo(
            SafeDeviceHandle DeviceInfoSet,
            string DeviceInstanceId,
            IntPtr hwndParent,
            int OpenFlags,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern int CM_Get_Device_ID(
            UInt32 dnDevInst,
            IntPtr buffer,
            int bufferLen,
            int flags
        );

        public const int IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX = 0x220448;
        public const int GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int OPEN_EXISTING = 0x3;
        public const int INVALID_HANDLE_VALUE = -1;

        public static string GUID_DEVINTERFACE_USB_DEVICE = "A5DCBF10-6530-11D2-901F-00C04FB951ED";

        // see https://docs.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-host-controller
        public static string GUID_DEVINTERFACE_USB_HOST_CONTROLLER = "3ABF6F2D-71C4-462A-8A92-1E6861E6AF27";


        public static int SPDRP_LOCATION_INFORMATION = 28;

        public const int DIGCF_PRESENT = 0x00000002;
        public const int DIGCF_DEVICEINTERFACE = 0x00000010;

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

        const int BUFFER_SIZE = 2048;
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DevicePath;
        }

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
            ref int RequiredSize,
            SP_DEVINFO_DATA DeviceInfoData
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

        const int IOCTL_USB_GET_ROOT_HUB_NAME = 0x220408;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct USB_ROOT_HUB_NAME
        {
            public int ActualLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string RootHubName;
        }

        public static string GetRootHub(string hostController)
        {
            using (var handle = CreateFile(
                hostController, 
                GENERIC_WRITE, 
                FILE_SHARE_WRITE, 
                IntPtr.Zero, 
                OPEN_EXISTING,
                0,
                IntPtr.Zero))
            {
                int nBytesReturned;
                USB_ROOT_HUB_NAME HubName = new USB_ROOT_HUB_NAME();
                int nBytes = Marshal.SizeOf(HubName);
                IntPtr pHubName = Marshal.AllocHGlobal(nBytes);

                var buf = new byte[1];
                int hubNameSize = 0;
                DeviceIoControl(
                    handle,
                    IOCTL_USB_GET_ROOT_HUB_NAME,
                    IntPtr.Zero,
                    0,
                    buf,
                    nBytes,
                    out hubNameSize,
                    IntPtr.Zero);

                var usbRootHubName2 = Marshal.PtrToStructure<USB_ROOT_HUB_NAME>(pHubName);


                var allocHGlobal = Marshal.AllocHGlobal(hubNameSize);
                DeviceIoControl(
                    handle,
                    IOCTL_USB_GET_ROOT_HUB_NAME,
                    IntPtr.Zero,
                    0,
                    allocHGlobal,
                    hubNameSize,
                    out hubNameSize,
                    IntPtr.Zero);

                var usbRootHubName = Marshal.PtrToStructure<USB_ROOT_HUB_NAME>(allocHGlobal);
            }

            return "";
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