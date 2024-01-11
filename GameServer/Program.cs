using System;
using System.Threading.Tasks;
using Network;

namespace GameServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ProtocolBinder.Instance.InitRegisterHandles();

            string serverUrl = "http://localhost:8080/";

            await WebSocketServer.Instance.StartServer(serverUrl);

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            WebSocketServer.Instance.StopServer();
        }
    }
}
