using Account;
using Network;
using Newtonsoft.Json;

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

            // 계정 정보 업데이트
            AccountManager.Instance.UpdateAccount(FirebaseManager.Instance.GetUID(), ack.UserID, ack.Level, ack.CurExp, ack.CurEnergy);
        }

        public void RegisterAccountHandler()
        {
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_ACCOUNT_GET_ACK, GS_ACCOUNT_GET_ACK);
        }
    }
}
