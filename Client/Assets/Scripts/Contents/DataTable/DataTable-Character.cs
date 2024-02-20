using FlexFramework.Excel;
using Framework.DataTable;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public enum CharacterType
{
    Knight = 1, 
    Mage, 
    Archer
}

namespace DataTable
{
    public class CharacterData
    {
        public int character_kind;
        public CharacterType character_type;
        public long health;
        public long attack;
        public long defense;
        public float critical_strike_rate;
        public List<int> buffs = new List<int>();
        public string sprite_name;
        public string name;
        public string desc;
    }

    // 타입과 엑셀 테이블 명칭을 맞춰주세요.
    public class CharacterDataTable : DataTableBase<CharacterDataTable>, IDataTable
    {
        // 한번 데이터가 쓰이면 변경할수 없다.
        // Clear 금지.

        // 시트 데이터.
        private Dictionary<int, CharacterData> m_common_character_data;

        public void LoadDataTable()
        {
            LoadCommonDataTable();
        }

        public void LoadCommonDataTable()
        {
            WorkBook book = GetCommonRowData();
            if (book.Contains("CHARACTER") == false)
            {
                Debug.LogError("ResourceDataTable - CHARACTER sheet not found");
                Application.Quit();
            }

            // Capacity 를 지정하여 딕셔너리 생성.
            m_common_character_data = new Dictionary<int, CharacterData>(book.GetRowCount("CHARACTER"));

            // 디버깅이 쉽도록 람다말고 함수를 넣어주세요.
            book.Foreach("CHARACTER", ParseCommonCharacterRowData);

            // 필요에 따라 추가해주세요.
        }

        public CharacterData GetCommonCharacterData(int in_character_kind)
        {
            m_common_character_data.TryGetValue(in_character_kind, out var out_data);

            return out_data;
        }

        public void ParseCommonCharacterRowData(Row in_row)
        {
            // 여기서 에러가 발생한다면 엑셀 쓰레기값을 확인해보자.
            var data = new CharacterData();
            data.character_kind = in_row[0].Integer;
            data.character_type = (CharacterType)Enum.Parse(typeof(CharacterType), in_row[1].ToString());
            data.health = in_row[2].Convert<long>();
            data.attack = in_row[3].Convert<long>();
            data.defense = in_row[4].Convert<long>();
            data.critical_strike_rate = in_row[5].Convert<float>();

            var buffs = in_row[6].ToSplitInteger();
            foreach (var buff in buffs)
                data.buffs.Add(buff);

            data.sprite_name = in_row[7].String;
            data.name = in_row[8].String;
            data.desc = in_row[9].String;

            m_common_character_data.Add(data.character_kind, data);
        }
    }
}