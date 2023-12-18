using FlexFramework.Excel;
using System.Collections.Generic;
using System.IO;

namespace DataTable
{
    public class AccountData
    {
        public int level;
        public long need_exp;
        public long max_energy;
    }

    public static class AccountDataTable
    {
        private static Dictionary<int, AccountData> m_account_data = new Dictionary<int, AccountData>();

        public readonly static string FilePath = "../DataTable/Common/AccountTable.xlsx";

        public static void LoadAccountDataTable()
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

                var account_data = new AccountData();
                account_data.level = row_data[0].Integer;
                account_data.need_exp = row_data[1].Integer;
                account_data.max_energy = row_data[2].Integer;

                m_account_data.Add(account_data.level, account_data);
            }

            fs.Close();
        }

        static public AccountData GetAccountData(int in_level)
        {
            m_account_data.TryGetValue(in_level, out var out_data);

            return out_data;
        }
    }
}