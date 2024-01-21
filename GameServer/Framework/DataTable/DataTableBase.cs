using FlexFramework.Excel;
using System.IO;
using System;

namespace Framework.DataTable
{

    public static class GlobalDataTablePath
    {
        public readonly static string COMMON_DATA_PATH = Path.Combine("..", "..", "..", "DataTable", "Common");
        public readonly static string SERVER_DATA_PATH = Path.Combine("..", "..", "..", "DataTable", "Server");
        // 클라는 서버에서 필요하지 않아 추가 안함.

        public readonly static string DATA_EXTENSION = ".xlsx";

        // 빌드된 파일에서 엑셀데이터를 로드하는 기능 추가필요.
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

            // 개발 단계에서 확실하게 체크하기위해
            // 데이터를 로드하지 못하면 종료처리.
            if (book == null) 
            {
                // 추후 종료 로직으로 변경해야한다.
                Environment.Exit(0);
            }

            return book;
        }

        private string GetCommonPath()
        {
            string path = Path.Combine(GlobalDataTablePath.COMMON_DATA_PATH, this.GetType().Name);

            path = path + GlobalDataTablePath.DATA_EXTENSION;

            // 빌드된 파일에서 엑셀데이터를 로드하는 기능 추가필요.

            return path;
        }

        private string GetServerPath()
        {
            string path = Path.Combine(GlobalDataTablePath.SERVER_DATA_PATH, this.GetType().Name);

            path = path + GlobalDataTablePath.DATA_EXTENSION;

            // 빌드된 파일에서 엑셀데이터를 로드하는 기능 추가필요.

            return path;
        }
    }

}