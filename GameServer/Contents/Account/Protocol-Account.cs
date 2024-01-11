using Newtonsoft.Json;

[System.Serializable]
public class GS_ACCOUNT_GET_REQ
{
    public string UserID;
}

[System.Serializable]
public class GS_ACCOUNT_GET_ACK
{
    public int Result;
    public int Level;
    public long Exp;
}

namespace Network
{
    public partial class ProtocolBinder
    {
        public static void GS_ACCOUNT_GET_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_ACCOUNT_GET_REQ>(in_message);
            if (req == null)
                return;

            var ack = new GS_ACCOUNT_GET_ACK();
            ack.Level = 1;
            ack.Exp = 10;
            ack.Result = 1;

            WebSocketServer.Instance.Send<GS_ACCOUNT_GET_ACK>(123, PROTOCOL.GS_ACCOUNT_GET_ACK, ack);
        }

        public void RegisterAccountHandler()
        {
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_ACCOUNT_GET_REQ, GS_ACCOUNT_GET_REQ);
        }
    }
}
