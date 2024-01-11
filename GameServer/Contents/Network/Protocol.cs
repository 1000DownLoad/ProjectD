using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Framework;

namespace Network
{
    public enum PROTOCOL
    {
        NONE,

        GS_ACCOUNT_GET_REQ,
        GS_ACCOUNT_GET_ACK,
    }

    public partial class ProtocolBinder : TSingleton<ProtocolBinder>
    {
        // Protocol-Contents 에 작성한 함수들 호출
        public void InitRegisterHandles()
        {
            RegisterAccountHandler();
        }
    }
}