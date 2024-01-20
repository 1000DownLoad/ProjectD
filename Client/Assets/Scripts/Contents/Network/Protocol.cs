using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Framework;

namespace Protocol
{
    public enum PROTOCOL
    {
        NONE,

        GS_ACCOUNT_GET_REQ,
        GS_ACCOUNT_GET_ACK,

        GS_USER_LOGIN_REQ,
        GS_USER_LOGIN_ACK,
        GS_USER_BASE_INFO_GET_REQ,
        GS_USER_BASE_INFO_GET_ACK,
    }

    public partial class ProtocolBinder : TSingleton<ProtocolBinder>
    {
        // Protocol-Contents 에 작성한 함수들 호출
        public void Initialize()
        {
            RegisterAccountHandler();
            RegisterUserHandler();
        }
    }
}