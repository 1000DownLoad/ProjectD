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
        public static void GS_ACCOUNT_GET_ACK(string in_message)
        {
            var account_get_ack = JsonConvert.DeserializeObject<GS_ACCOUNT_GET_ACK>(in_message);
            if (account_get_ack == null)
                return;
        }

        public void RegisterAccountHandler()
        {
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_ACCOUNT_GET_ACK, GS_ACCOUNT_GET_ACK);
        }
    }
}
