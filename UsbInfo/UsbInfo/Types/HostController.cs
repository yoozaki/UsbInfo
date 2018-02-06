namespace UsbInfo.Types
{
    public class HostController
    {
        public string RootHubPath { get; }
        public string HostControllerPath { get; }

        public HostController(
            string hostControllerPath,
            string rootHubName)
        {
            HostControllerPath = hostControllerPath;
            RootHubPath = @"\\.\" + rootHubName;
        }
    }
}