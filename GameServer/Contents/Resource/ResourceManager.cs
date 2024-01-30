using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using DataBase;
using ResourceDatas = System.Collections.Generic.Dictionary<ResourceType, long>;

class ResourceManager : TSingleton<ResourceManager>
{
    private ConcurrentDictionary<long, ResourceDatas> m_user_resource_datas = new ConcurrentDictionary<long, ResourceDatas>();

    public void InsertResource(long in_user_id, ResourceType in_resource_type, long in_count)
    {
        if (in_count == 0)
            // TODO : 에러 로그 추가.
            return;

        var resoure_datas = GetOrCreateUserResourceDatas(in_user_id);
        if (resoure_datas.ContainsKey(in_resource_type))
        {
            resoure_datas[in_resource_type] += in_count;
        }
        else
        {
            // 없는 경우 새로추가.
            resoure_datas.Add(in_resource_type, in_count);
        }
    }

    private ResourceDatas GetOrCreateUserResourceDatas(long in_user_id)
    {
        // 데이터가 이미 존재하는지 체크.
        if (m_user_resource_datas.TryGetValue(in_user_id, out var out_resoure_datas))
            return out_resoure_datas;

        ResourceDatas new_resource_datas = new ResourceDatas();
        bool ret = m_user_resource_datas.TryAdd(in_user_id, new_resource_datas);
        if (ret == false)
        {
            // 실패하는 경우는 아래와 같다.
            //  1. 키 중복 또는 null 키.
            //  2. 용량 초과.
            // 여기서 키 대한 실패는 고려하지 않는다. 상위에서 체크를 맡긴다.
            Console.WriteLine("CreateAndInsertUserResourceData Fail");

            // 더이상 문제가 발생하는것을 막기위해 프로그램 종료.
            Environment.Exit(1);
        }

        return new_resource_datas;
    }

    public long GetResourceCount(long in_user_id, ResourceType in_resource_type)
    {
        if (m_user_resource_datas.TryGetValue(in_user_id, out var out_resoure_datas) == false)
            return 0;

        out_resoure_datas.TryGetValue(in_resource_type, out var out_count);
        return out_count;
    }

    // 현재 서버에 있는 데이터를 DB 에 반영한다.
    // 초기 개발이기 때문에 성능을 고려하지 않고 DB 테이블 전체를 갱신한다.
    // 업데이트에 실패한 경우에 대한 고려가 필요하다.
    public void UpdateDB(long in_user_id)
    {
        // 데이터가 이미 존재하는지 체크.
        if (m_user_resource_datas.TryGetValue(in_user_id, out var out_resoure_datas) == false)
            return;

        Dictionary<string, object> data = new Dictionary<string, object>();
        foreach (var resource_data in out_resoure_datas)
        {
            // 문자열 변환이 비용이 싸지 않을거 같아 확인이 필요.
            data.Add(resource_data.Key.ToString(), resource_data.Value);
        }

        // 현재 DB.Collection.Document 의 값이 string 이라 user id 를 문자열로 넘겨준다.
        DataBaseManager.Instance.UpdateDataBase("T_Resource_Info", in_user_id.ToString(), data);
    }

    public void FetchDB(long in_user_id)
    {
        var resource_datas = DataBaseManager.Instance.GetDocumentData("T_Resource_Info", in_user_id.ToString());
        if (resource_datas == null)
            return;

        foreach (var resource_data in resource_datas)
        {
            if (Enum.TryParse(resource_data.Key, out ResourceType out_resource_type) == false)
            {
                // 더이상 문제가 발생하는것을 막기위해 프로그램 종료.
                Environment.Exit(1);
            }

            InsertResource(in_user_id, out_resource_type, Convert.ToInt64(resource_data.Value));
        }
    }
}
