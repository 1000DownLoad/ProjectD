using FlexFramework.Excel;
using System.Collections.Generic;
using System.IO;

namespace DataTable
{
    public class AccountTableData
    {
        public int level;
        public long max_exp;
        public long max_energy;
    }

    public static class AccountDataTable
    {
        private static Dictionary<int, AccountTableData> m_account_data = new Dictionary<int, AccountTableData>();
        public readonly static string FilePath = "../../../DataTable/Common/AccountDataTable.xlsx";

        public static void LoadCommonAccountDataTable()
        {
            m_account_data.Clear();

            var fs = new FileStream(FilePath, FileMode.Open);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);

            WorkBook book = new WorkBook(bytes);

            var doc = book["ACCOUNT"];

            for (int row = 1; row < doc.Rows.Count; row++)
            {
                var row_data = doc.Rows[row];

                var account_data = new AccountTableData();
                account_data.level = row_data[0].Integer;
                account_data.max_exp = row_data[1].Integer;
                account_data.max_energy = row_data[2].Integer;

                m_account_data.Add(account_data.level, account_data);
            }

            fs.Close();
        }

        static public AccountTableData GetAccountTableData(int in_level)
        {
            m_account_data.TryGetValue(in_level, out var out_data);

            return out_data;
        }
    }
}