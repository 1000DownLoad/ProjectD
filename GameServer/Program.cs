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
            AccountDataTable.LoadCommonAccountDataTable();

            // DataBase Initialize
            DataBaseManager.Instance.Initialize();

            // ProtocolBinder Initialize
            ProtocolBinder.Instance.Initialize();

            // Socket
            WebSocketServer.Instance.Initialize();

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            WebSocketServer.Instance.StopServer();
        }
    }
}
