using Newtonsoft.Json;
using Network;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_USER_ITEM_FETCH_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_USER_ITEM_FETCH_REQ>(in_message);
            if(req == null)
                return;

            var user = UserManager.Instance.GetUser(req.UserID);
            if(user == null) 
                return;

            var ack = UserItemManager.Instance.CreateFetchProtocolStruct(req.UserID);
            if(ack == null) 
                return;

            WebSocketServer.Instance.Send<GS_USER_ITEM_FETCH_ACK>(ack.UserID, PROTOCOL.GS_USER_ITEM_FETCH_ACK, ack);
        }

        public void RegisterItemHandler()
        {
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_ITEM_FETCH_REQ, GS_USER_ITEM_FETCH_REQ);
        }
    }
}
