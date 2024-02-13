using Network;
using Newtonsoft.Json;
using UnityEngine;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_USER_AUTH_TOKEN_ACK(string in_message)
        {
            var ack = JsonConvert.DeserializeObject<GS_USER_AUTH_TOKEN_ACK>(in_message);
            if (ack == null)
                return;

            UserManager.Instance.m_auth_token = ack.AuthToken;

            // 인증 토큰으로 로그인
            FirebaseManager.Instance.LoginWithCustomToken(ack.AuthToken);
        }

        public static void GS_USER_LOGIN_ACK(string in_message)
        {
            var ack = JsonConvert.DeserializeObject<GS_USER_LOGIN_ACK>(in_message);
            if (ack == null)
                return;

            // 유저 데이터 세팅
            if (UserManager.Instance.GetUser() == null)
            {
                var new_user = new UserInfo();
                new_user.account_id = ack.AccountID;
                new_user.user_id = ack.UserID;
                new_user.level = ack.Level;
                new_user.exp = ack.Exp;

                UserManager.Instance.SetUser(new_user);
            }
            else
            {
                UserManager.Instance.UpdateData(ack.AccountID, ack.UserID, ack.Level, ack.Exp);
            }

            GUIManager.Instance.PublishEvnet(new EVENT_USER_DATA_UPDATE(ack.Level, ack.Exp));
        }

        public static void GS_USER_BASE_INFO_GET_ACK(string in_message)
        {
            var ack = JsonConvert.DeserializeObject<GS_USER_BASE_INFO_GET_ACK>(in_message);
            if (ack == null)
                return;

            // 유저 자원 데이터
            if(ack.ResourceDatas.Count > 0)
            {
                UserResourceManager.Instance.UpdateData(ack.ResourceDatas);
                GUIManager.Instance.PublishEvnet(new EVENT_USER_RESOURCE_DATA_UPDATE(ack.ResourceDatas));
            }

            // 유저 아이템 데이터
            if (ack.ItemDatas.Count > 0)
            {
                UserItemManager.Instance.UpdateData(ack.ItemDatas);
                GUIManager.Instance.PublishEvnet(new EVENT_USER_ITEM_DATA_UPDATE(ack.ItemDatas));
            }

            // 유저 데이터 세팅 완료
            UserManager.Instance.SetInitData(ack.Result == 1);
        }

        public static void GS_USER_COMMAND_ACK(string in_message)
        {
            var ack = JsonConvert.DeserializeObject<GS_USER_COMMAND_ACK>(in_message);
            if (ack == null)
                return;

            if(ack.Result != 0)
                Debug.Log("치트 성공");
            else
                Debug.Log("치트 실패");
        }

        public void RegisterUserHandler()
        {
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_AUTH_TOKEN_ACK, GS_USER_AUTH_TOKEN_ACK);
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_LOGIN_ACK, GS_USER_LOGIN_ACK);
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_BASE_INFO_GET_ACK, GS_USER_BASE_INFO_GET_ACK);
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_COMMAND_ACK, GS_USER_COMMAND_ACK);
        }
    }
}
