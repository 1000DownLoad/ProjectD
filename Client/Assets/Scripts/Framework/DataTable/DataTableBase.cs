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
        // 서버는 클라이언트에서 필요하지 않아 추가 안함.

        public readonly static string DATA_EXTENSION = ".xlsx";
#else
        public readonly static string COMMON_DATA_PATH = Path.Combine("DataTable", "Common");
        public readonly static string CLIENT_DATA_PATH = Path.Combine("DataTable", "Client");
        // 서버는 클라이언트에서 필요하지 않아 추가 안함.
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
            

            // 사용하지 않는 리소스 정리
            Resources.UnloadUnusedAssets();
#endif

            // 개발 단계에서 확실하게 체크하기위해
            // 데이터를 로드하지 못하면 종료처리.
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
            // 빌드 환경에서는 Resoure 를 통해 데이터를 가지고 오기 때문에 확장자를 붙이지 않는다.
#endif

            return path;
        }

        private string GetClientPath()
        {
            string path = Path.Combine(GlobalDataTablePath.CLIENT_DATA_PATH, this.GetType().Name);

#if UNITY_EDITOR
            path = path + GlobalDataTablePath.DATA_EXTENSION;
#else
            // 빌드 환경에서는 Resoure 를 통해 데이터를 가지고 오기 때문에 확장자를 붙이지 않는다.
#endif

            return path;
        }
    }

}