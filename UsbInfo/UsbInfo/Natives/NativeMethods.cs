using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using UsbInfo.Natives.Types;

namespace UsbInfo.Natives
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
    internal static class NativeMethods
    {
        internal const int IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX = 0x00220448;
        internal const int IOCTL_USB_GET_NODE_INFORMATION = 0x00220408;
        internal const int IOCTL_USB_GET_ROOT_HUB_NAME = 0x00220408;
        internal const int IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME = 0x00220420;

        // see https://docs.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-host-controller
        private static readonly Guid GUID_DEVINTERFACE_USB_HOST_CONTROLLER =
            new Guid("3ABF6F2D-71C4-462A-8A92-1E6861E6AF27");

        // see https://docs.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-device
        internal static readonly Guid GUID_DEVINTERFACE_USB_DEVICE = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED");

        // see https://docs.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-hub
        internal static readonly Guid GUID_DEVINTERFACE_USB_HUB = new Guid("F18A0E88-C30C-11D0-8815-00A0C906BED8");

        // we use registry value. see https://msdn.microsoft.com/en-us/library/windows/desktop/ms724872(v=vs.85).aspx
        public const int BUFFER_SIZE = 2048;

        private const int SPDRP_DEVICEDESC = 0x0000_0000;
        private const int SPDRP_DRIVER = 0x0000_0009;

        private const int DIGCF_PRESENT = 0x0000_0002;
        private const int DIGCF_DEVICEINTERFACE = 0x0000_0010;

        internal const int GENERIC_WRITE = 0x4000_0000;
        internal const int FILE_SHARE_WRITE = 0x000_0002;
        internal const int OPEN_EXISTING = 0x0000_0003;

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public readonly string DevicePath;
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

        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern SafeDeviceHandle SetupDiGetClassDevs(
            ref Guid classGuid,
            IntPtr enumerator,
            IntPtr hwndParent,
            int flags
        );

        [DllImport("setupapi.dll")]
        internal static extern bool SetupDiDestroyDeviceInfoList(
            IntPtr deviceInfoSet
        );

        [DllImport("setupapi.dll")]
        private static extern bool SetupDiEnumDeviceInterfaces(
            SafeDeviceHandle DeviceInfoSet,
            IntPtr DeviceInfoData,
            ref Guid InterfaceClassGuid,
            int MemberIndex,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            IntPtr DeviceInterfaceDetailData,
            int DeviceInterfaceDetailDataSize,
            ref int RequiredSize,
            IntPtr DeviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData,
            int DeviceInterfaceDetailDataSize,
            ref int RequiredSize,
            ref SP_DEVINFO_DATA DeviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern bool SetupDiGetDeviceRegistryProperty(
            SafeDeviceHandle DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData,
            int iProperty,
            ref int PropertyRegDataType,
            StringBuilder PropertyBuffer,
            int PropertyBufferSize,
            ref int RequiredSize
        );

        private const int CM_LOCATE_DEVNODE_NORMAL = 0x00000000;
        [DllImport("setupapi.dll")]
        internal static extern PnpConfigrationResult CM_Locate_DevNode(out uint pdnDevInst, string pDeviceID, int ulFlags = CM_LOCATE_DEVNODE_NORMAL);

        [DllImport("setupapi.dll")]
        internal static extern PnpConfigrationResult CM_Get_Child(out uint pdnDevInst, uint dnDevInst, int ulFlags = 0);

        [DllImport("setupapi.dll")]
        internal static extern PnpConfigrationResult CM_Get_Sibling(out uint pdnDevInst, uint dnDevInst, int ulFlags = 0);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern PnpConfigrationResult CM_Get_Device_ID(uint dnDevInst, StringBuilder Buffer, int BufferSize, int ulFlags = 0);

        [DllImport("setupapi.dll")]
        internal static extern PnpConfigrationResult CM_Get_Device_ID_Size(out int pulLen, uint dnDevInst, int uflags = 0);


        internal static void ThrowIfSetLastError(bool result)
        {
            if (!result)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        internal static void ThrowIfNotCrSuccess(PnpConfigrationResult result)
        {
            if (result != PnpConfigrationResult.CR_SUCCESS)
            {
                throw new PnpConfigrationException(result);
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

        private static int GetDeviceInterfaceDetailDataSize(
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