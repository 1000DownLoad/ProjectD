using Account;
using Network;
using Newtonsoft.Json;

[System.Serializable]
public class GS_ACCOUNT_GET_REQ
{
    public string AccountID;
}

[System.Serializable]
public class GS_ACCOUNT_GET_ACK
{
    public int  Result;
    public long UserID;
    public int  Level;
    public long CurExp;
    public long CurEnergy;
}

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_ACCOUNT_GET_ACK(string in_message)
        {
            var ack = JsonConvert.DeserializeObject<GS_ACCOUNT_GET_ACK>(in_message);
            if (ack == null)
                return;

            if (ack.Result != 1)
                return;

            AccountManager.Instance.UpdateAccount(FirebaseManager.Instance.GetAccountID(), ack.UserID, ack.Level, ack.CurExp, ack.CurEnergy);
        }

        public void RegisterAccountHandler()
        {
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_ACCOUNT_GET_ACK, GS_ACCOUNT_GET_ACK);
        }
    }
}
