using UnityEngine;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Framework;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Timers;
using Protocol;

namespace Network
{
    public class WebSocketClient : TSingleton<WebSocketClient>
    {
        private ClientWebSocket m_web_socket = new ClientWebSocket();
        private Queue<string> m_packet_queue = new Queue<string>();
        private Dictionary<PROTOCOL, Action<string>> m_protocol_handlers = new Dictionary<PROTOCOL, Action<string>>();
        private System.Timers.Timer m_packet_process_timer;

        protected override void OnCreateSingleton()
        {
            m_packet_process_timer = new System.Timers.Timer(100);
            m_packet_process_timer.Elapsed += OnPacketRecvProcessUpdate;
            m_packet_process_timer.AutoReset = true;
            m_packet_process_timer.Start();
        }

        private void OnPacketRecvProcessUpdate(object sender, ElapsedEventArgs e)
        {
            if (m_packet_queue.Count < 1)
                return;

            RecvProcessPacket(m_packet_queue.Dequeue());
        }

        public async void Connect(string in_url, string in_account_id)
        {
            try
            {
                Uri url = new Uri(in_url);
                m_web_socket.Options.SetRequestHeader("ACCOUNT_ID", in_account_id);
                await m_web_socket.ConnectAsync(url, CancellationToken.None);
                Debug.Log("WebSocket connected!");

                await Receive();
            }
            catch (Exception e)
            {
                Debug.LogError($"WebSocket error: {e.Message}");
            }
        }

        public WebSocketState GetSocketState()
        {
            return m_web_socket.State;
        }

        private async Task Receive()
        {
            var buffer = new byte[4096];
            while (m_web_socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await m_web_socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text && result.EndOfMessage)
                {
                    string packet = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    m_packet_queue.Enqueue(packet);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await m_web_socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
            }
        }

        private void RecvProcessPacket(string in_packet)
        {
            var packet_data = JsonConvert.DeserializeObject<Dictionary<string, object>>(in_packet);

            int protocol_id = Convert.ToInt32(packet_data["ProtocolID"]);
            string message = packet_data["Message"].ToString();

            if (m_protocol_handlers.TryGetValue((PROTOCOL)protocol_id, out Action<string> handler))
            {
                handler.Invoke(message);
            }
        }

        public void RegisterProtocolHandler(PROTOCOL in_protocol, Action<string> in_event_handle)
        {
            m_protocol_handlers[in_protocol] = in_event_handle;
        }

        public async Task Send<T>(PROTOCOL in_protocol, T in_packet)
        {
            if (m_web_socket.State == WebSocketState.Open)
            {
                int protocol_id = (int)in_protocol;
                string message = JsonConvert.SerializeObject(in_packet);

                var packet_data = new Dictionary<string, object>
                {
                    { "ProtocolID", protocol_id },
                    { "Message", message },
                };

                string packet = JsonConvert.SerializeObject(packet_data);
                byte[] buffer = Encoding.UTF8.GetBytes(packet);

                await m_web_socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                Debug.Log("SocketState Closed");
            }
        }

        private async void Close()
        {
            if (m_web_socket.State == WebSocketState.Open)
            {
                await m_web_socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                m_web_socket.Dispose();
            }
        }
    }
}