using System.IO;
using UnityEngine;
using FlexFramework.Excel;

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
        public readonly static string COMMON_DATA_PATH = Path.Combine("DataTable", "Common");
        public readonly static string CLIENT_DATA_PATH = Path.Combine("DataTable", "Client");
        // ������ Ŭ���̾�Ʈ���� �ʿ����� �ʾ� �߰� ����.
#endif
    }

    public interface IDataTable
    {
        public void LoadDataTable();
    }


    public class DataTableBase<T> : TSingleton<T> where T : class, new()
    {
        protected WorkBook GetCommonRowData() 
        {
            WorkBook book = null;
#if UNITY_EDITOR
            var fs = new FileStream(GetCommonPath(), FileMode.Open);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);

            book = new WorkBook(bytes);
#else
            TextAsset text_asset = Resources.Load<TextAsset>(GetCommonPath());

            if (text_asset != null) 
                book = new WorkBook(text_asset.bytes);
            

            // ������� �ʴ� ���ҽ� ����
            Resources.UnloadUnusedAssets();
#endif

            // ���� �ܰ迡�� Ȯ���ϰ� üũ�ϱ�����
            // �����͸� �ε����� ���ϸ� ����ó��.
            if (book == null) 
            {
                Debug.LogError(GetCommonPath() + " common data file not found!");
                Application.Quit(1);
            }

            return book;
        }

        private string GetCommonPath()
        {
            string path = Path.Combine(GlobalDataTablePath.COMMON_DATA_PATH, this.GetType().Name);

#if UNITY_EDITOR
            path = path + GlobalDataTablePath.DATA_EXTENSION;
#else
            // ���� ȯ�濡���� Resoure �� ���� �����͸� ������ ���� ������ Ȯ���ڸ� ������ �ʴ´�.
#endif

            return path;
        }

        private string GetClientPath()
        {
            string path = Path.Combine(GlobalDataTablePath.CLIENT_DATA_PATH, this.GetType().Name);

#if UNITY_EDITOR
            path = path + GlobalDataTablePath.DATA_EXTENSION;
#else
            // ���� ȯ�濡���� Resoure �� ���� �����͸� ������ ���� ������ Ȯ���ڸ� ������ �ʴ´�.
#endif

            return path;
        }
    }

}