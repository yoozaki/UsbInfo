namespace UsbInfo.Types
{
    public class HostController
    {
        public string RootHubPath { get; }
        public string HostControllerPath { get; }

        public HostController(
            string hostControllerPath,
            string rootHubPath)
        {
            HostControllerPath = hostControllerPath;
            RootHubPath = rootHubPath;
        }
    }
}