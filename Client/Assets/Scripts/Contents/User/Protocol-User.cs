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

            // 유저 데이터 세팅
            if (UserManager.Instance.GetUser() == null)
            {
                var new_user = new UserInfo();
                new_user.account_id = ack.AccountID;
                new_user.user_id = ack.UserID;
                new_user.level = ack.Level;
                new_user.cur_exp = ack.CurExp;
                new_user.cur_energy = ack.CurEnergy;

                UserManager.Instance.SetUser(new_user);
            }
            else
            {
                UserManager.Instance.UpdateUser(ack.AccountID, ack.UserID, ack.Level, ack.CurExp, ack.CurEnergy);
            }

            // UI OnEventHandle 실행
            GUIManager.Instance.PublishEvnet(new EVENT_USER_DATA_UPDATE(ack.Level, ack.CurExp, ack.CurEnergy));
        }

        public static void GS_USER_BASE_INFO_GET_ACK(string in_message)
        {
            var ack = JsonConvert.DeserializeObject<GS_USER_BASE_INFO_GET_ACK>(in_message);
            if (ack == null)
                return;

            // 유저 초기 데이터 세팅
            UserManager.Instance.SetInitData(ack.Result == 1);
        }

        public void RegisterUserHandler()
        {
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_LOGIN_ACK, GS_USER_LOGIN_ACK);
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_BASE_INFO_GET_ACK, GS_USER_BASE_INFO_GET_ACK);
        }
    }
}
