using Network;
using Newtonsoft.Json;
using UnityEngine;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_USER_RESOURCE_UPDATE_NFY(string in_message) 
        {
            var req = JsonConvert.DeserializeObject<GS_USER_RESOURCE_UPDATE_NFY>(in_message);
            if (req == null)
                return;

            UserResourceManager.Instance.UpdateData(req.UpdateDatas);

            GUIManager.Instance.PublishEvnet(new EVENT_USER_RESOURCE_DATA_UPDATE(req.UpdateDatas));
        }

        public void RegisterResourceHandler() 
        {
            WebSocketClient.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_RESOURCE_UPDATE_NFY, GS_USER_RESOURCE_UPDATE_NFY);
        }
    }
}