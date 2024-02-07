using System;
using System.Threading.Tasks;
using Network;
using DataBase;
using DataTable;
using Protocol;

namespace GameServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // DataTable Load
            UserDataTable.Instance.LoadDataTable();

            // FireBase Initialize
            FirebaseManager.Instance.Initialize();

            // Database Initialize
            DatabaseManager.Instance.Initialize();

            // ProtocolBinder Initialize
            ProtocolBinder.Instance.Initialize();

            // CommandManager Initialize
            CommandManager.Instance.Initialize();

            // Socket
            await WebSocketServer.Instance.Initialize();

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            WebSocketServer.Instance.StopServer();
        }
    }
}
