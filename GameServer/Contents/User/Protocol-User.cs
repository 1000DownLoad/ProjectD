using Account;
using User;
using Network;
using Newtonsoft.Json;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_USER_LOGIN_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_USER_LOGIN_REQ>(in_message);
            if (req == null)
                return;

            // 계정 정보 확인
            var account = AccountManager.Instance.GetAccountByUserID(req.UserID);
            if (account == null)
                return;

            // 유저 정보 확인
            var user = UserManager.Instance.GetUser(account.user_id);
            if(user == null)
                user = UserManager.Instance.InsertUser(account.user_id);

            // 로그인 시 DB 에서 유저 정보를 가져와서 세팅합니다.
            if(UserManager.Instance.LoadDataBaseData(user.user_id) == false)
            {
                // DB에 유저 데이터가 없다면 추가한 유저 데이터를 DB에 저장
                UserManager.Instance.UpdateDataBase(user.user_id);
            }
                
            var ack = new GS_USER_LOGIN_ACK();
            ack.UserID = account.user_id;
            ack.Result = 1;

            WebSocketServer.Instance.Send<GS_USER_LOGIN_ACK>(ack.UserID, PROTOCOL.GS_USER_LOGIN_ACK, ack);
        }

        public static void GS_USER_BASE_INFO_GET_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_USER_BASE_INFO_GET_REQ>(in_message);
            if (req == null)
                return;

            var ack = new GS_USER_BASE_INFO_GET_ACK();
            ack.Result = 1;

            WebSocketServer.Instance.Send<GS_USER_BASE_INFO_GET_ACK>(req.UserID, PROTOCOL.GS_USER_BASE_INFO_GET_ACK, ack);
        }

        public void RegisterUserHandler()
        {
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_LOGIN_REQ, GS_USER_LOGIN_REQ);
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_BASE_INFO_GET_REQ, GS_USER_BASE_INFO_GET_REQ);
        }
    }
}
