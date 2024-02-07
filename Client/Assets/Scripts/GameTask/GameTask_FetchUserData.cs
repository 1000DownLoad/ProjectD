using System;
using UnityEngine;
using Framework.Scheduler.Base;
using Network;
using Protocol;

class GameTask_FetchUserData : Task
{
    private bool try_fetch_user_resource = false;

    public override void OnAddTask() {}

    public override void OnAwake()
    {
        // 최초 시작단계에서 관련 프로퍼티 초기화.
        try_fetch_user_resource = false;


    }

    public override void OnUpdate() 
    {
        var user = UserManager.Instance.GetUser();
        if (user == null) 
        {
            Debug.LogError("GameTask_FetchUserData - try_fetch_user_resource, user is null");
            return;
        }

        if (TryFetchResourceData(user.user_id) == false)
            return;

        Complete(ETaskState.Success);
    }

    private bool TryFetchResourceData(long in_user_id)
    {
        // 일정 시간 간격으로 Try 하는 로직이 필요.
        if (try_fetch_user_resource == false) 
        {            
            var req = new GS_USER_RESOURCE_FETCH_REQ();
            req.UserID = in_user_id;

            WebSocketClient.Instance.Send<GS_USER_RESOURCE_FETCH_REQ>(PROTOCOL.GS_USER_RESOURCE_FETCH_REQ, req);

            try_fetch_user_resource = true;
        }

        if (UserResourceManager.Instance.Isinitialized() == false)
            return false;

        return true;
    }

    public override void OnComplete() { }
}