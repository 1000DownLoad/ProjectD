using System.IO;


namespace Framework.DataTable
{

    public static class GlobalDataTablePath
    {
#if UNITY_EDITOR
        public readonly static string COMMON_DATA_PATH = Path.Combine("..", "DataTable", "Common");
        public readonly static string CLIENT_DATA_PATH = Path.Combine("..", "DataTable", "Client");
        // ������ Ŭ���̾�Ʈ���� �ʿ����� �ʾ� �߰� ����.

        public readonly static string DATA_EXTENSION = ".xlsx";
#else
        public readonly static string COMMON_DATA_PATH = Path.Combine("Resource", "DataTable", "Common");
        public readonly static string CLIENT_DATA_PATH = Path.Combine("Resource", "DataTable", "Client");
        // ������ Ŭ���̾�Ʈ���� �ʿ����� �ʾ� �߰� ����.

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