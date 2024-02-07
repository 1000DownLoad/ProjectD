using Network;
using Newtonsoft.Json;
using UnityEngine;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_USER_RESOURCE_FETCH_ACK(string in_message) 
        {
            var req = JsonConvert.DeserializeObject<GS_USER_RESOURCE_FETCH_ACK>(in_message);
            if (req == null)
                return;

            var user = UserManager.Instance.GetUser();
            if (user.user_id != req.UserID)
            {
                Debug.LogError($"GS_USER_RESOURCE_FETCH_ACK not match user id. Cur user id : {user.user_id} Req user id : req.UserID");
                return;
            }

            if (req.FetchDatas == null) 
            {
                Debug.LogError($"GS_USER_RESOURCE_FETCH_ACK FetchDatas null. user id : {req.UserID}");
                return;
            }
            
            UserResourceManager.Instance.Fectch(req.FetchDatas);
        }

        public void RegisterResourceHandler() 
        {
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_RESOURCE_FETCH_ACK, GS_USER_RESOURCE_FETCH_ACK);
        }
    }
}