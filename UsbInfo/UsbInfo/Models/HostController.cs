namespace UsbInfo.Models
{
    internal class HostController
    {
        public string RootHubPath { get; }
        public string HostControllerPath { get; }

        public HostController(
            string hostControllerPath,
            string devicePath)
        {
            HostControllerPath = hostControllerPath;
            RootHubPath = devicePath;
        }
    }
}