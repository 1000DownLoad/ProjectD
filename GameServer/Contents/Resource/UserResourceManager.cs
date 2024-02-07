using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using DataBase;
using ResourceDatas = System.Collections.Generic.Dictionary<ResourceType, long>;

class UserResourceManager : TSingleton<UserResourceManager>
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

            if (resoure_datas[in_resource_type] < 0)
                resoure_datas[in_resource_type] = 0;
        }
        else
        {
            if (in_count < 0)
                in_count = 0;

            // 없는 경우 새로추가.
            resoure_datas.Add(in_resource_type, in_count);
        }
    }

    // 유저 생성시 초기 데이터
    public void CreateUserInitData(long in_user_id)
    {
        InsertResource(in_user_id, ResourceType.FATIGUE, 100);
        InsertResource(in_user_id, ResourceType.GOLD, 1000);
        UpdateDB(in_user_id);
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
            data.Add(resource_data.Key.ToString(), resource_data.Value);
        }

        DatabaseManager.Instance.UpdateField("T_Resource_Info", in_user_id.ToString(), data);
    }

    public void UpdateDB(long in_user_id, ResourceType in_type)
    {
        DatabaseManager.Instance.UpdateField("T_Resource_Info", in_user_id.ToString(), in_type.ToString(), GetResourceCount(in_user_id, in_type));
    }

    public void FetchDB(long in_user_id)
    {
        var resource_datas = DatabaseManager.Instance.GetDocumentData("T_Resource_Info", in_user_id.ToString());
        if (resource_datas == null)
            return;

        // DB 데이터를 가져오기 전 기존 데이터 제거
        Clear(in_user_id);

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

    public void Clear(long in_user_id)
    {
        m_user_resource_datas.TryRemove(in_user_id, out var value);
    }

    public GS_USER_RESOURCE_FETCH_ACK CreateFetchProtocolStruct(long in_user_id)
    {
        GS_USER_RESOURCE_FETCH_ACK ret = new GS_USER_RESOURCE_FETCH_ACK();

        m_user_resource_datas.TryGetValue(in_user_id, out var resource_datas);

        ret.UserID = in_user_id;

        // 원본을 던져도 될까.
        ret.FetchDatas = resource_datas;

        return ret;
    }
}
