using System;
using UnityEngine;
using Framework.Scheduler.Base;
using Network;
using Protocol;

class GameTask_FetchUserData : Task
{
    private bool is_send_user_base_info = false;

    public override void OnAwake()
    {
        Debug.Log("GameTask_FetchUserData OnAwake");
    }

    public override void OnUpdate() 
    {
        var user = UserManager.Instance.GetUser();
        if (user == null) 
            return;

        // 유저 기본 정보 요청
        if (is_send_user_base_info == false)
        {
            var user_req = new GS_USER_BASE_INFO_GET_REQ();
            user_req.UserID = user.user_id;
            WebSocketClient.Instance.Send<GS_USER_BASE_INFO_GET_REQ>(PROTOCOL.GS_USER_BASE_INFO_GET_REQ, user_req);

            is_send_user_base_info = true;
        }

        // 유저 정보 초기화 체크
        if (UserManager.Instance.m_is_init_data == false)
            return;

        Complete(ETaskState.Success);
    }

    public override void OnComplete() 
    {
        Debug.Log("GameTask_FetchUserData OnComplete");
    }
}