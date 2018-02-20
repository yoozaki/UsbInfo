using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace UsbInfo.Natives
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    public class NativeMethods
    {
        internal const int IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX = 0x00220448;
        internal const int IOCTL_USB_GET_NODE_INFORMATION = 0x00220408;
        internal const int IOCTL_USB_GET_ROOT_HUB_NAME = 0x00220408;
        internal const int IOCTL_USB_GET_NODE_CONNECTION_NAME = 0x00220414;
        internal const int IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME = 0x00220420;

        // see https://docs.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-host-controller
        internal static readonly Guid GUID_DEVINTERFACE_USB_HOST_CONTROLLER =
            new Guid("3ABF6F2D-71C4-462A-8A92-1E6861E6AF27");

        // see https://docs.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-device
        internal static readonly Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");

        // see https://docs.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-hub
        internal static readonly Guid GUID_DEVINTERFACE_USB_HUB = new Guid("F18A0E88-C30C-11D0-8815-00A0C906BED8");

        // we use registry value. see https://msdn.microsoft.com/en-us/library/windows/desktop/ms724872(v=vs.85).aspx
        internal const int BUFFER_SIZE = 2048;

        internal const int SPDRP_DEVICEDESC = 0x0000_0000;
        internal const int SPDRP_DRIVER = 0x0000_0009;

        internal const int DIGCF_PRESENT = 0x0000_0002;
        internal const int DIGCF_DEVICEINTERFACE = 0x0000_0010;

        internal const int GENERIC_WRITE = 0x4000_0000;
        internal const int FILE_SHARE_READ = 0x0000_0001;
        internal const int FILE_SHARE_WRITE = 0x000_0002;
        internal const int OPEN_EXISTING = 0x0000_0003;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct USB_DEVICE_DESCRIPTOR
        {
            public byte bLength;
            public byte bDescriptorType;
            public ushort bcdUSB;
            public byte bDeviceClass;
            public byte bDeviceSubClass;
            public byte bDeviceProtocol;
            public byte bMaxPacketSize0;
            public ushort idVendor;
            public ushort idProduct;
            public ushort bcdDevice;
            public byte iManufacturer;
            public byte iProduct;
            public byte iSerialNumber;
            public byte bNumConfigurations;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct USB_NODE_CONNECTION_INFORMATION_EX
        {
            public uint ConnectionIndex;
            public USB_DEVICE_DESCRIPTOR DeviceDescriptor;
            public byte CurrentConfigurationValue;
            public byte Speed;
            public byte DeviceIsHub;
            public ushort DeviceAddress;
            public uint NumberOfOpenPipes;
            public uint ConnectionStatus;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct USB_NODE_CONNECTION_DRIVERKEY_NAME
        {
            public uint ConnectionIndex;
            public uint ActualLength;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DriverKeyName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct USB_ROOT_HUB_NAME
        {
            public uint ActualLength;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string RootHubName;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct USB_NODE_INFORMATION
        {
            public int NodeType;
            public USB_NODE_UNION UsbNodeUnion;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct USB_NODE_UNION
        {
            [FieldOffset(0)] public USB_HUB_INFORMATION HubInformation;
            [FieldOffset(0)] public USB_MI_PARENT_INFORMATION MiParentInformation;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct USB_MI_PARENT_INFORMATION
        {
            public int NumberOfInterfaces;
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
        internal static extern SafeFileHandle CreateFile(
            string lpFileName,
            int dwDesiredAccess,
            int dwShareMode,
            IntPtr lpSecurityAttributes,
            int dwCreationDisposition,
            int dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool DeviceIoControl(
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
        internal static extern bool DeviceIoControl(
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
        internal static extern bool SetupDiOpenDeviceInfo(
            SafeDeviceHandle DeviceInfoSet,
            string DeviceInstanceId,
            IntPtr hwndParent,
            int OpenFlags,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern SafeDeviceHandle SetupDiGetClassDevs(
            ref Guid classGuid,
            IntPtr enumerator,
            IntPtr hwndParent,
            int flags
        );

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern SafeDeviceHandle SetupDiGetClassDevs(
            IntPtr classGuid,
            IntPtr enumerator,
            IntPtr hwndParent,
            int flags
        );

        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiDestroyDeviceInfoList(
            IntPtr deviceInfoSet
        );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetupDiEnumDeviceInfo(
            SafeDeviceHandle DeviceInfoSet,
            int MemberIndex,
            ref SP_DEVINFO_DATA deviceData
        );

        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiEnumDeviceInterfaces(
            SafeDeviceHandle DeviceInfoSet,
            IntPtr DeviceInfoData,
            ref Guid InterfaceClassGuid,
            int MemberIndex,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            IntPtr DeviceInterfaceDetailData,
            int DeviceInterfaceDetailDataSize,
            ref int RequiredSize,
            IntPtr DeviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData,
            int DeviceInterfaceDetailDataSize,
            ref int RequiredSize,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData,
            int DeviceInterfaceDetailDataSize,
            ref int RequiredSize, SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            int iProperty,
            ref int PropertyRegDataType,
            StringBuilder PropertyBuffer,
            int PropertyBufferSize,
            ref int RequiredSize
        );

        internal enum CrResult
        {

            CR_SUCCESS = (0x00000000),
            CR_DEFAULT = (0x00000001),
            CR_OUT_OF_MEMORY = (0x00000002),
            CR_INVALID_POINTER = (0x00000003),
            CR_INVALID_FLAG = (0x00000004),
            CR_INVALID_DEVNODE = (0x00000005),
            CR_INVALID_DEVINST = CR_INVALID_DEVNODE,
            CR_INVALID_RES_DES = (0x00000006),
            CR_INVALID_LOG_CONF = (0x00000007),
            CR_INVALID_ARBITRATOR = (0x00000008),
            CR_INVALID_NODELIST = (0x00000009),
            CR_DEVNODE_HAS_REQS = (0x0000000A),
            CR_DEVINST_HAS_REQS = CR_DEVNODE_HAS_REQS,
            CR_INVALID_RESOURCEID = (0x0000000B),
            CR_DLVXD_NOT_FOUND = (0x0000000C), // WIN 95 ONLY
            CR_NO_SUCH_DEVNODE = (0x0000000D),
            CR_NO_SUCH_DEVINST = CR_NO_SUCH_DEVNODE,
            CR_NO_MORE_LOG_CONF = (0x0000000E),
            CR_NO_MORE_RES_DES = (0x0000000F),
            CR_ALREADY_SUCH_DEVNODE = (0x00000010),
            CR_ALREADY_SUCH_DEVINST = CR_ALREADY_SUCH_DEVNODE,
            CR_INVALID_RANGE_LIST = (0x00000011),
            CR_INVALID_RANGE = (0x00000012),
            CR_FAILURE = (0x00000013),
            CR_NO_SUCH_LOGICAL_DEV = (0x00000014),
            CR_CREATE_BLOCKED = (0x00000015),
            CR_NOT_SYSTEM_VM = (0x00000016), // WIN 95 ONLY
            CR_REMOVE_VETOED = (0x00000017),
            CR_APM_VETOED = (0x00000018),
            CR_INVALID_LOAD_TYPE = (0x00000019),
            CR_BUFFER_SMALL = (0x0000001A),
            CR_NO_ARBITRATOR = (0x0000001B),
            CR_NO_REGISTRY_HANDLE = (0x0000001C),
            CR_REGISTRY_ERROR = (0x0000001D),
            CR_INVALID_DEVICE_ID = (0x0000001E),
            CR_INVALID_DATA = (0x0000001F),
            CR_INVALID_API = (0x00000020),
            CR_DEVLOADER_NOT_READY = (0x00000021),
            CR_NEED_RESTART = (0x00000022),
            CR_NO_MORE_HW_PROFILES = (0x00000023),
            CR_DEVICE_NOT_THERE = (0x00000024),
            CR_NO_SUCH_VALUE = (0x00000025),
            CR_WRONG_TYPE = (0x00000026),
            CR_INVALID_PRIORITY = (0x00000027),
            CR_NOT_DISABLEABLE = (0x00000028),
            CR_FREE_RESOURCES = (0x00000029),
            CR_QUERY_VETOED = (0x0000002A),
            CR_CANT_SHARE_IRQ = (0x0000002B),
            CR_NO_DEPENDENT = (0x0000002C),
            CR_SAME_RESOURCES = (0x0000002D),
            CR_NO_SUCH_REGISTRY_KEY = (0x0000002E),
            CR_INVALID_MACHINENAME = (0x0000002F), // NT ONLY
            CR_REMOTE_COMM_FAILURE = (0x00000030), // NT ONLY
            CR_MACHINE_UNAVAILABLE = (0x00000031), // NT ONLY
            CR_NO_CM_SERVICES = (0x00000032), // NT ONLY
            CR_ACCESS_DENIED = (0x00000033), // NT ONLY
            CR_CALL_NOT_IMPLEMENTED = (0x00000034),
            CR_INVALID_PROPERTY = (0x00000035),
            CR_DEVICE_INTERFACE_ACTIVE = (0x00000036),
            CR_NO_SUCH_DEVICE_INTERFACE = (0x00000037),
            CR_INVALID_REFERENCE_STRING = (0x00000038),
            CR_INVALID_CONFLICT_LIST = (0x00000039),
            CR_INVALID_INDEX = (0x0000003A),
            CR_INVALID_STRUCTURE_SIZE = (0x0000003B),
            NUM_CR_RESULTS = (0x0000003C)
        };


        internal const int CM_LOCATE_DEVNODE_NORMAL = 0x00000000;
        [DllImport("setupapi.dll")]
        internal static extern CrResult CM_Locate_DevNode(out uint pdnDevInst, string pDeviceID, int ulFlags = CM_LOCATE_DEVNODE_NORMAL);

        [DllImport("setupapi.dll")]
        internal static extern CrResult CM_Get_Child(out uint pdnDevInst, uint dnDevInst, int ulFlags = 0);

        [DllImport("setupapi.dll")]
        internal static extern CrResult CM_Get_Sibling(out uint pdnDevInst, uint dnDevInst, int ulFlags = 0);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern CrResult CM_Get_Device_ID(uint dnDevInst, StringBuilder Buffer, int BufferSize, int ulFlags = 0);

        [DllImport("setupapi.dll")]
        internal static extern CrResult CM_Get_Device_ID_Size(out int pulLen, uint dnDevInst, int uflags = 0);


        internal static void ThrowIfSetLastError(bool result)
        {
            if (!result)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        internal class CrException : Exception
        {
            public CrResult CrResult { get; }

            public CrException(CrResult result)
            {
                CrResult = result;
            }
        }

        internal static void ThrowIfNotCrSuccess(CrResult result)
        {
            if (result != CrResult.CR_SUCCESS)
            {
                throw new CrException(result);
            }
        }

        internal static IEnumerable<SP_DEVINFO_DATA> HostControllers()
        {
            var guid = GUID_DEVINTERFACE_USB_HOST_CONTROLLER;
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

        internal static IEnumerable<string> HostControllerPaths()
        {
            var classGuid = GUID_DEVINTERFACE_USB_HOST_CONTROLLER;
            foreach (var deviceMetaData in EnumerableDeviceMetaData(classGuid))
            {
                yield return deviceMetaData.DevicePath;
            }
        }

        internal static IEnumerable<DeviceMetaData> EnumerableDeviceMetaData(Guid classGuid)
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

                    var deviceInfoData = new SP_DEVINFO_DATA {cbSize = Marshal.SizeOf<SP_DEVINFO_DATA>()};
                    if (SetupDiGetDeviceInterfaceDetail(
                        hostHandles,
                        ref deviceInstace,
                        ref deviceInstaceDetail,
                        deviceInterfaceDetailDataSize,
                        ref deviceInterfaceDetailDataSize,
                        ref deviceInfoData))
                    {
                        yield return new DeviceMetaData(
                            GetDeviceProperty(SPDRP_DEVICEDESC, hostHandles, deviceInfoData),
                            GetDeviceProperty(SPDRP_DRIVER, hostHandles, deviceInfoData), 
                            deviceInstaceDetail.DevicePath);
                    }
                }
            }
        }

        private static string GetDeviceProperty(
            int propertyId,
            SafeDeviceHandle hostHandles, 
            SP_DEVINFO_DATA deviceInfoData)
        {
            var propertyBuffer = new StringBuilder(BUFFER_SIZE);
            int actualSize = 0;
            int regType = 1;
            SetupDiGetDeviceRegistryProperty(
                hostHandles,
                ref deviceInfoData,
                propertyId,
                ref regType,
                propertyBuffer,
                propertyBuffer.Capacity,
                ref actualSize);

            return propertyBuffer.ToString();
        }

        internal static int GetDeviceInterfaceDetailDataSize(
            SafeDeviceHandle hostHandles,
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