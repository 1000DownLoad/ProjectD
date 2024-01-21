using FlexFramework.Excel;
using System.IO;
using System;

namespace Framework.DataTable
{

    public static class GlobalDataTablePath
    {
        public readonly static string COMMON_DATA_PATH = Path.Combine("..", "..", "..", "DataTable", "Common");
        public readonly static string SERVER_DATA_PATH = Path.Combine("..", "..", "..", "DataTable", "Server");
        // Ŭ��� �������� �ʿ����� �ʾ� �߰� ����.

        public readonly static string DATA_EXTENSION = ".xlsx";

        // ����� ���Ͽ��� ���������͸� �ε��ϴ� ��� �߰��ʿ�.
    }

    public interface IDataTable
    {
        void LoadDataTable();
    }


    public class DataTableBase<T> : TSingleton<T> where T : class, new()
    {
        protected WorkBook GetCommonRowData() 
        {
            WorkBook book = null;

            var fs = new FileStream(GetCommonPath(), FileMode.Open);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);

            book = new WorkBook(bytes);

            // ���� �ܰ迡�� Ȯ���ϰ� üũ�ϱ�����
            // �����͸� �ε����� ���ϸ� ����ó��.
            if (book == null) 
            {
                // ���� ���� �������� �����ؾ��Ѵ�.
                Environment.Exit(0);
            }

            return book;
        }

        private string GetCommonPath()
        {
            string path = Path.Combine(GlobalDataTablePath.COMMON_DATA_PATH, this.GetType().Name);

            path = path + GlobalDataTablePath.DATA_EXTENSION;

            // ����� ���Ͽ��� ���������͸� �ε��ϴ� ��� �߰��ʿ�.

            return path;
        }

        private string GetServerPath()
        {
            string path = Path.Combine(GlobalDataTablePath.SERVER_DATA_PATH, this.GetType().Name);

            path = path + GlobalDataTablePath.DATA_EXTENSION;

            // ����� ���Ͽ��� ���������͸� �ε��ϴ� ��� �߰��ʿ�.

            return path;
        }
    }

}