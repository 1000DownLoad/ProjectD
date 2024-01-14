#  DataTable 빌드 및 패치에 관해서

필요 사전 지식
- Unity 의 Resouce 폴더는 빌드후 변경이 불가능하다.
- Unity 는 .xlsx(엑셀) 파일을 인식하지 못한다.
- 외부 리소스의 경우 Application.persistentDataPath 에서 데이터를 가져올 수 잇다.


목표
- Resource/DataTable 에 DataTable 들을 복사하여 런타임에 데이터를 로드 및 사용한다.
- ~~패치가 가능하도록 버전을 기록한다.~~ (미구현 - 희망 사항)
- ~~DataTable 패치 데이터가 있다면  Application.persistentDataPath 에 저장한다.~~ (미구현 - 희망 사항)
- ~~버전을 비교하여 어떤 NDT 데이터를 로드하여 사용할지 구분한다.~~ (미구현 - 희망 사항)
- 이후 로드된 데이터를 런타임에 사용한다.


동작 
- 빌드 단계에서 DataTable 엑셀 데이터를 Resource/DataTable 폴더에 복사한다.
	- Resource/DataTable 이 없다면 폴더를 새로 생성.
	- Resource/DataTable 가 있다면 폴더 제거 및 생성.
- 복사하는 과정에서 확장자를 .text 로 변경한다. (파일을 인식하지 못하기 때문에)
- 빌드 진행.
- ~~Resource 폴더에 저장된 데이터 버전과 원격 에 올라가있는 데이터 버전 정보를 비교한다.~~ (미구현 - 희망 사항)
- ~~원격의 데이터 버전이 높다면 다운로드 후  Application.persistentDataPath  에 저장한다.~~ (미구현 - 희망 사항)
- ~~버전을 비교하여 데이터 로드 경로를 변경한다.~~ (미구현 - 희망 사항)
- 데이터 로드 경로를 통해 DataTable 데이터 들을 세팅한다.


고려 사항.
- ~~패치 단계에서 데이터를 통쨰로 받지 않도록 구분해야함.~~ (미구현 - 희망사항)


모든 과정은 아직 테스트를 진행하지 못해봤습니다.
개발 과정에서 내용이 변경될 수 있습니다.