using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Timers;
using Protocol;
using System.Collections.Concurrent;

namespace Network
{
    public class WebSocketServer : TSingleton<WebSocketServer>
    {
        private HttpListener m_http_listener;
        private ConcurrentDictionary<long, WebSocket> m_user_sockets = new ConcurrentDictionary<long, WebSocket>();
        private ConcurrentQueue<string> m_packet_queue = new ConcurrentQueue<string>();
        private Dictionary<PROTOCOL, Action<string>> m_protocol_handlers = new Dictionary<PROTOCOL, Action<string>>();
        private System.Timers.Timer m_packet_process_timer;

        public async Task Initialize()
        {
            string serverUrl = "http://localhost:8080/";

            m_http_listener = new HttpListener();
            m_http_listener.Prefixes.Add(serverUrl);

            m_http_listener.Start();
            StartTimer();

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

        private void OnPacketRecvProcessUpdate(object in_sender, ElapsedEventArgs in_event_args)
        {
            if (m_packet_queue.Count < 1)
                return;

            if (m_packet_queue.TryDequeue(out string packet) == false)
                return;

            RecvProcessPacket(packet);
        }

        private void StartTimer()
        {
            m_packet_process_timer = new System.Timers.Timer(100);
            m_packet_process_timer.Elapsed += OnPacketRecvProcessUpdate;
            m_packet_process_timer.AutoReset = true;
            m_packet_process_timer.Start();
        }

        private async void HandleWebSocketRequest(HttpListenerContext in_context)
        {
            HttpListenerWebSocketContext webSocketContext = null;

            try
            {
                webSocketContext = await in_context.AcceptWebSocketAsync(subProtocol: null);

                string account_id = webSocketContext.Headers.Get("ACCOUNT_ID");
                var user = UserManager.Instance.GetUserByAccountID(account_id);
                if (user == null)
                {
                    // DB에서 유저 정보를 가져옵니다.
                    user = UserManager.Instance.FetchDB(account_id);
                    if (user == null)
                    {
                        // 유저 생성
                        user = UserManager.Instance.CreateUser(account_id);

                        // 생성된 유저 데이터로 DB 갱신
                        UserManager.Instance.UpdateDB(account_id);
                    }
                    else
                    {
                        // DB에서 가져온 정보를 매니저에 넣어줍니다.
                        UserManager.Instance.InsertUser(user);
                    }
                }

                if (user.user_id == 0)
                {
                    webSocketContext?.WebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal Server Error", CancellationToken.None);
                    return;
                }

                // 유저의 모든 데이터 DB로 부터 가져오기
                UserManager.Instance.UserDataFetchDB(user.user_id);

                m_user_sockets.TryAdd(user.user_id, webSocketContext.WebSocket);

                await Receive(user.user_id, webSocketContext.WebSocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
                webSocketContext?.WebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal Server Error", CancellationToken.None);
            }
        }

        private async Task Receive(long in_user_id, WebSocket in_web_socket)
        {
            var buffer = new byte[4096];
            while (in_web_socket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await in_web_socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text && result.EndOfMessage)
                {
                    string packet = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    m_packet_queue.Enqueue(packet);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    m_user_sockets.TryRemove(in_user_id, out var out_value);
                    await in_web_socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
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

        public void Send<T>(long in_user_id, PROTOCOL in_protocol, T in_packet)
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

                out_web_socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
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

            foreach (var socket in m_user_sockets)
            {
                socket.Value.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                socket.Value.Dispose();
            }
        }
    }
}