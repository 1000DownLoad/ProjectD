# Server
클라이언트/서버간 연결 방식

# 세부 동작
### 1. 클라이언트
- WebSocketClient에서 소켓을 관리합니다.
- m_web_socket : 현재 연결되어 있는 소켓
- m_packet_queue : 서버로 부터 받은 패킷을 큐에 담아두고 순차적으로 실행하기 위한 컨테이너
- m_protocol_handlers : 프로토콜 ID에 따라 실행될 함수들을 저장

### 2. 서버
- WebSocketServer에서 소켓들을 관리합니다.
- m_http_listener : Http 리스너
- m_user_sockets : 연결된 유저들의 소켓들을 관리합니다.
- m_packet_queue : 클라로 부터 받은 패킷을 큐에 담아두고 순차적으로 실행하기 위한 컨테이너
- m_protocol_handlers : 프로토콜 ID에 따라 실행될 함수들을 저장

### 3. 패킷
- Newtonsoft.Json.JsonConvert를 사용해서 Json 데이터를 주고 받음 (Dictionary 컨테이너도 직렬화/역직렬화 가능)
- 패킷 형태는 ProtocolID, Message를 키값으로 갖는 Dictionary<string, object> 형태의 패킷을 주고 받습니다.
- ProtocolID는 Protocol.PROTOCOL에 정의된 프로토콜들을 사용합니다.
- Message의 벨류값으로 사용되는 구조체는 Protocol-Struct에 정의한 구조체들을 사용합니다. (서버/클라 동일한 구조 사용해야함) 