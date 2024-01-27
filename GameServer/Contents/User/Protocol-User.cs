﻿using User;
using Network;
using Newtonsoft.Json;

namespace Protocol
{
    public partial class ProtocolBinder
    {
        public static void GS_USER_LOGIN_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_USER_LOGIN_REQ>(in_message);
            if (req == null)
                return;

            // 유저 정보 확인
            var user = UserManager.Instance.GetUserByAccountID(req.AccountID);
            if (user == null)
                return;
                
            var ack = new GS_USER_LOGIN_ACK();
            ack.AccountID = user.account_id;
            ack.UserID = user.user_id;
            ack.Level = user.level;
            ack.CurExp = user.cur_exp;
            ack.CurEnergy = user.cur_energy;
            ack.Result = 1;

            WebSocketServer.Instance.Send<GS_USER_LOGIN_ACK>(ack.UserID, PROTOCOL.GS_USER_LOGIN_ACK, ack);
        }

        public static void GS_USER_BASE_INFO_GET_REQ(string in_message)
        {
            var req = JsonConvert.DeserializeObject<GS_USER_BASE_INFO_GET_REQ>(in_message);
            if (req == null)
                return;

            var ack = new GS_USER_BASE_INFO_GET_ACK();
            ack.Result = 1;

            WebSocketServer.Instance.Send<GS_USER_BASE_INFO_GET_ACK>(req.UserID, PROTOCOL.GS_USER_BASE_INFO_GET_ACK, ack);
        }

        public void RegisterUserHandler()
        {
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_LOGIN_REQ, GS_USER_LOGIN_REQ);
            WebSocketServer.Instance.RegisterProtocolHandler(PROTOCOL.GS_USER_BASE_INFO_GET_REQ, GS_USER_BASE_INFO_GET_REQ);
        }
    }
}
