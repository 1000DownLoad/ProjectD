using Network;
using Newtonsoft.Json;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_USER_AUTH_TOKEN_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_USER_AUTH_TOKEN_REQ>(in_message);
            if (req == null)
                return;

            // 유저 정보 확인
            var user = UserManager.Instance.GetUserByAccountID(req.AccountID);
            if (user == null)
                return;

            var ack = new GS_USER_AUTH_TOKEN_ACK();
            ack.Result = 1;
            ack.AuthToken = FirebaseManager.Instance.CreateAuthToken(req.AccountID).Result;

            WebSocketServer.Instance.Send<GS_USER_AUTH_TOKEN_ACK>(user.user_id, PROTOCOL.GS_USER_AUTH_TOKEN_ACK, ack);
        }

        public static void GS_USER_LOGIN_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_USER_LOGIN_REQ>(in_message);
            if (req == null)
                return;

            // 유저 정보 확인
            var user = UserManager.Instance.GetUserByAccountID(req.AccountID);
            if (user == null)
                return;
                
            var ack = new GS_USER_LOGIN_ACK();
            ack.AccountID = user.account_id;
            ack.UserID = user.user_id;
            ack.Level = user.level;
            ack.Exp = user.exp;
            ack.Result = 1;

            WebSocketServer.Instance.Send<GS_USER_LOGIN_ACK>(user.user_id, PROTOCOL.GS_USER_LOGIN_ACK, ack);
        }

        public static void GS_USER_BASE_INFO_GET_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_USER_BASE_INFO_GET_REQ>(in_message);
            if (req == null)
                return;

            var ack = new GS_USER_BASE_INFO_GET_ACK();
            ack.Result = 1;

            // 유저 자원 데이터
            var resource_datas = UserResourceManager.Instance.GetResourceDatas(req.UserID);
            if (resource_datas != null)
                ack.ResourceDatas = resource_datas;

            // 유저 아이템 데이터
            var item_datas = UserItemManager.Instance.GetItemDatas(req.UserID);
            if (item_datas != null)
                ack.ItemDatas = item_datas;

            WebSocketServer.Instance.Send<GS_USER_BASE_INFO_GET_ACK>(req.UserID, PROTOCOL.GS_USER_BASE_INFO_GET_ACK, ack);
        }

        public static void GS_USER_COMMAND_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_USER_COMMAND_REQ>(in_message);
            if (req == null)
                return;

            // 문자열 분리
            string[] split_str = req.Command.Split(' ');

            if (split_str.Length <= 0)
                return;

            // 첫 번째 부분은 명령키
            string command_key = split_str[0];

            // 명령어 실행
            CommandManager.Instance.InvokeCommand(command_key, req.UserID, req.Command);

            var ack = new GS_USER_COMMAND_ACK();
            ack.Result = 1;

            WebSocketServer.Instance.Send<GS_USER_COMMAND_ACK>(req.UserID, PROTOCOL.GS_USER_COMMAND_ACK, ack);
        }

        public void RegisterUserHandler()
        {
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_AUTH_TOKEN_REQ, GS_USER_AUTH_TOKEN_REQ);
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_LOGIN_REQ, GS_USER_LOGIN_REQ);
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_BASE_INFO_GET_REQ, GS_USER_BASE_INFO_GET_REQ);
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_COMMAND_REQ, GS_USER_COMMAND_REQ);
        }
    }
}
