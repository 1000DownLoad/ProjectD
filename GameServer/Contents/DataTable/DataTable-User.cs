using FlexFramework.Excel;
using System.Collections.Generic;
using Framework.DataTable;
using System;

namespace DataTable
{
    public class LevelTableData
    {
        public int  level;
        public long exp;
        public long fatigue_point;
    }

    // 타입과 엑셀 테이블 명칭을 맞춰주세요.
    public class UserDataTable : DataTableBase<UserDataTable>, IDataTable
    {
        // 한번 데이터가 쓰이면 변경할수 없다.
        // Clear 금지.

        // account 시트 데이터.
        private Dictionary<int, LevelTableData> m_common_level_data;

        public void LoadDataTable()
        {
            LoadCommonDataTable();
        }

        public void LoadCommonDataTable()
        {
            WorkBook book = GetCommonRowData();
            if (book.Contains("LEVEL") == false)
            {
                // 추후 종료 로직으로 변경해야한다.
                Environment.Exit(0);
            }

            // Capacity 를 지정하여 딕셔너리 생성.
            m_common_level_data = new Dictionary<int, LevelTableData>(book.GetRowCount("LEVEL"));

            // 디버깅이 쉽도록 람다말고 함수를 넣어주세요.
            book.Foreach("LEVEL", ParseCommonAccountRowData);

            // 필요에 따라 추가해주세요
        }

        public LevelTableData GetLevelTableData(int in_level)
        {
            m_common_level_data.TryGetValue(in_level, out var out_data);

            return out_data;
        }


        public void ParseCommonAccountRowData(Row in_row)
        {
            // 여기서 에러가 발생한다면 엑셀에 쓰레기값을 확인해보자.
            var level_data = new LevelTableData();
            level_data.level = in_row[0].Integer;
            level_data.exp = in_row[1].Integer;
            level_data.fatigue_point = in_row[2].Integer;

            m_common_level_data.Add(level_data.level, level_data);
        }
    }
}