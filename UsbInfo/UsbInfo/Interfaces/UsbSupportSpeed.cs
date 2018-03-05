using JetBrains.Annotations;

namespace UsbInfo.Interfaces
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public enum UsbSupportSpeed

    {
        LowSpeed = 0,
        FullSpeed,
        HighSpeed,
        SuperSpeed
    }
}