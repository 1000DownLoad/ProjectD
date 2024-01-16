using FlexFramework.Excel;
using Framework.DataTable;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace DataTable
{
    public class AccountTableData
    {
        public int  level;
        public long max_exp;
        public long max_energy;
    }

    // 타입과 엑셀 테이블 명칭을 맞춰주세요.
    public class AccountDataTable : DataTableBase<AccountDataTable>
    {
        private Dictionary<int, AccountTableData> m_common_account_data = new Dictionary<int, AccountTableData>();

        public void LoadCommonAccountDataTable()
        {
            m_common_account_data.Clear();

            WorkBook book = GetCommonRowData();

            var doc = book["ACCOUNT"];

            for (int row = 1; row < doc.Rows.Count; row++)
            {
                var row_data = doc.Rows[row];

                var account_data = new AccountTableData();
                account_data.level = row_data[0].Integer;
                account_data.max_exp = row_data[1].Integer;
                account_data.max_energy = row_data[2].Integer;

                m_common_account_data.Add(account_data.level, account_data);
            }
        }

        public AccountTableData GetAccountTableData(int in_level)
        {
            m_common_account_data.TryGetValue(in_level, out var out_data);

            return out_data;
        }
    }
}