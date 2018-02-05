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
}