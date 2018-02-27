using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;

namespace UsbInfo.Natives
{
    [UsedImplicitly]
    internal class SafeDeviceHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeDeviceHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle() 
            => NativeMethods.SetupDiDestroyDeviceInfoList(handle);
    }
}