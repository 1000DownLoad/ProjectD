using Network;
using Newtonsoft.Json;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        //public static void GS_USER_RESOURCE_FETCH_REQ(string in_message)
        //{
        //    var req = JsonConvert.DeserializeObject<GS_USER_RESOURCE_FETCH_REQ>(in_message);
        //    if (req == null)
        //        return;

        //    // 유저가 존재하는지 체크
        //    var user = UserManager.Instance.GetUser(req.UserID);
        //    if (user == null)
        //    {
        //        // 에러 로그 추가 필요.
        //        return;
        //    }

        //    WebSocketServer.Instance.Send<GS_USER_RESOURCE_FETCH_ACK>(ack.UserID, PROTOCOL.GS_USER_RESOURCE_FETCH_ACK, ack);
        //}

        public void RegisterResourceHandler()
        {
            //WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_RESOURCE_FETCH_REQ, GS_USER_RESOURCE_FETCH_REQ);
        }
    }
}
