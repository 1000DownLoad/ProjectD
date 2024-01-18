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
        // 한번 데이터가 쓰이면 변경할수 없다.
        // Clear 금지.
        private Dictionary<int, AccountTableData> m_common_account_data;

        public void LoadCommonAccountDataTable()
        {
            WorkBook book = GetCommonRowData();
            if (book.Contains("ACCOUNT") == false)
            {
                Debug.LogError("AccountDataTable - ACCOUNT sheet not found");
                Application.Quit();
            }

            // Capacity 를 지정하여 딕셔너리 생성.
            m_common_account_data = new Dictionary<int, AccountTableData>(book.GetRowCount("ACCOUNT"));

            // 디버깅이 쉽도록 람다말고 함수를 넣어주세요.
            book.Foreach("ACCOUNT", PraseCommonAccountRowData);
        }

        public AccountTableData GetAccountTableData(int in_level)
        {
            m_common_account_data.TryGetValue(in_level, out var out_data);

            return out_data;
        }


        public void PraseCommonAccountRowData(Row in_row) 
        {
            // 여기서 에러가 발생한다면 엑셀에 쓰레기값을 확인해보자.
            var account_data = new AccountTableData();
            account_data.level = in_row[0].Integer;
            account_data.max_exp = in_row[1].Integer;
            account_data.max_energy = in_row[2].Integer;

            m_common_account_data.Add(account_data.level, account_data);
        }
    }
}