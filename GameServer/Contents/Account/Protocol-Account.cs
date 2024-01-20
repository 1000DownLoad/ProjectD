using Network;
using Newtonsoft.Json;
using Account;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_ACCOUNT_GET_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_ACCOUNT_GET_REQ>(in_message);
            if (req == null)
                return;

            var account = AccountManager.Instance.GetAccount(req.AccountID);
            if (account == null)
                return;

            var ack = new GS_ACCOUNT_GET_ACK();
            ack.Result = 1;
            ack.UserID = account.user_id;
            ack.Level = account.level;
            ack.CurExp = account.cur_exp;
            ack.CurEnergy = account.cur_energy;

            WebSocketServer.Instance.Send<GS_ACCOUNT_GET_ACK>(account.user_id, PROTOCOL.GS_ACCOUNT_GET_ACK, ack);
        }

        public void RegisterAccountHandler()
        {
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_ACCOUNT_GET_REQ, GS_ACCOUNT_GET_REQ);
        }
    }
}
