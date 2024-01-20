using Account;
using Network;
using Newtonsoft.Json;
using User;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_USER_LOGIN_ACK(string in_message)
        {
            var ack = JsonConvert.DeserializeObject<GS_USER_LOGIN_ACK>(in_message);
            if (ack == null)
                return;

            // 유저 ID 세팅
            UserManager.Instance.SetUserID(ack.UserID);
        }

        public static void GS_USER_BASE_INFO_GET_ACK(string in_message)
        {
            var ack = JsonConvert.DeserializeObject<GS_USER_BASE_INFO_GET_ACK>(in_message);
            if (ack == null)
                return;

            // 유저 초기 데이터 세팅
            UserManager.Instance.SetUserInitData(ack.Result == 1);
        }

        public void RegisterUserHandler()
        {
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_LOGIN_ACK, GS_USER_LOGIN_ACK);
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_BASE_INFO_GET_ACK, GS_USER_BASE_INFO_GET_ACK);
        }
    }
}
