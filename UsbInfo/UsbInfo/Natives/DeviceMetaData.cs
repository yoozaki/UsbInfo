namespace UsbInfo.Natives
{
    internal class DeviceMetaData
    {
        public string DeviceDescription { get; }
        public string DevicePath { get; }
        public string DriverKeyName { get; }

        public DeviceMetaData(string deviceDescription, string driverKeyName, string devicePath)
        {
            DeviceDescription = deviceDescription;
            DevicePath = devicePath;
            DriverKeyName = driverKeyName;
        }
    }
}