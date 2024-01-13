using System.IO;


namespace Framework.DataTable
{

    public static class GlobalDataTablePath
    {
#if UNITY_EDITOR
        public readonly static string COMMON_DATA_PATH = Path.Combine("..", "DataTable", "Common");
        public readonly static string CLIENT_DATA_PATH = Path.Combine("..", "DataTable", "Client");
        // 서버는 클라이언트에서 필요하지 않아 추가 안함.

        public readonly static string DATA_EXTENSION = ".xlsx";
#else
        public readonly static string COMMON_DATA_PATH = Path.Combine("Resource", "DataTable", "Common");
        public readonly static string CLIENT_DATA_PATH = Path.Combine("Resource", "DataTable", "Client");
        // 서버는 클라이언트에서 필요하지 않아 추가 안함.

        public readonly static string DATA_EXTENSION = ".text";
#endif
    }


    public class DataTableBase<T> : TSingleton<T> where T : class, new()
    {
        protected string GetCommonPath()
        {
            return Path.Combine(GlobalDataTablePath.COMMON_DATA_PATH
                , this.GetType().Name + GlobalDataTablePath.DATA_EXTENSION);
        }

        protected string GetClientPath()
        {
            return Path.Combine(GlobalDataTablePath.CLIENT_DATA_PATH
                , this.GetType().Name + GlobalDataTablePath.DATA_EXTENSION);
        }
    }

}