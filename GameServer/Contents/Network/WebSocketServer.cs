using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Timers;

namespace Network
{
    public class WebSocketServer : TSingleton<WebSocketServer>
    {
        private HttpListener m_http_listener;
        private Dictionary<long, WebSocket> m_user_sockets = new Dictionary<long, WebSocket>();
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

        public async Task StartServer(string in_url)
        {
            m_http_listener = new HttpListener();
            m_http_listener.Prefixes.Add(in_url);

            m_http_listener.Start();
            Console.WriteLine("Server started. Waiting for connections...");

            while (true)
            {
                var context = await m_http_listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    HandleWebSocketRequest(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private async void HandleWebSocketRequest(HttpListenerContext context)
        {
            HttpListenerWebSocketContext webSocketContext = null;

            try
            {
                webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
                var userId = 123;
                m_user_sockets[userId] = webSocketContext.WebSocket;

                await Receive(webSocketContext.WebSocket, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
                webSocketContext?.WebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal Server Error", CancellationToken.None);
            }
        }

        private async Task Receive(WebSocket webSocket, long userId)
        {
            var buffer = new byte[4096];
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text && result.EndOfMessage)
                {
                    string packet = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    m_packet_queue.Enqueue(packet);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    m_user_sockets.Remove(userId);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
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

        public async Task Send<T>(long in_user_id, PROTOCOL in_protocol, T in_packet)
        {
            if (m_user_sockets.TryGetValue(in_user_id, out WebSocket out_web_socket))
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

                await out_web_socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                Console.WriteLine($"User with ID {in_user_id} not found.");
            }
        }

        public void RegisterProtocolHandler(PROTOCOL in_protocol_id, Action<string> in_handler)
        {
            m_protocol_handlers[in_protocol_id] = in_handler;
        }

        public void StopServer()
        {
            m_http_listener.Stop();
            m_http_listener.Close();

            foreach(var data in m_user_sockets)
            {
                data.Value.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
        }
    }
}