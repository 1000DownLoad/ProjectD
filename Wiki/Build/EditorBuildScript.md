# Editor Buidl Script
***

빌드 과정에서 필요한 추가적인 동작이 구현 되어있는 스크립트.


# Build Script Info
***

ProjectD 에서 사용하는 빌드 스크립트 정보는 아래와 같다.

- BuildScript.cs
	- AndroidBuild
	- iOSBuild


# 동작 이미지
***

![빌드 흐름도](https://github.com/1000DownLoad/ProjectD/assets/16574400/aca02fbf-06c5-4fb5-bcac-01837af84a4f)


# 필요 사전 지식
***
DataTable
- dll 관련 정보.
	- 엑셀 정보를 읽기 위해 WorkBook 을 사용하는데 한글 환경에서 제작된 엑셀은 949 인코딩을 필요로 한다.
	- dll 을 따로 관리 할 수 없기 때문에 Unity Eidtor Mono 의 dll 을 복사하여 사용한다.
		- **만약 엔진 버전이 달라지는 경우 복사에 실패할 수 있다.**
- Unity 는 .xlsx(엑셀) 파일을 인식하지 못하기 떄문에 .txt 로 변경해야한다.
- **한글을 지원하지 않는 엑셀을 만들거나 CSV-UTF8 와 같은 형태로 데이터를 따로 관리하는 방법도 있지만, 이 과정도 만만치 않아 dll 을 포함한다.**


# 세부 동작
***
## 1. Copy Encoding Dll
엑셀 데이터를 읽기 위해서 949 인코딩을 포함하는 단계.
```
참고 사항 : dll 을 관리하기 어렵다 판단하여 Unity Editor Mono 의 인코딩 dll 을 복사하여 사용한다.
```



Unity Editor Mono 패스에서 dll 을 복사 후 프로젝트의 Plugin 폴더에 복사한다.	
```
Editor Path 는 EditorApplication.applicationPath 를 사용하며 나머지 경로는 손으로 확인 해가며 세팅 해두었다.
이로 인해 엔진 버전이 변경으로 경로가 변경되면 문제가 발생한다.
```


## 2. Copy DataTable

Resource/DataTable 에 DataTable 들을 복사하여 런타임에 데이터를 로드 및 사용한다.

```
DataTable 은 클라만의 데이터가 아니기 때문에 외부에 존재합니다.
하지만 Build 파일에서 이 파일을 사용하기 위해서는 Resouce 하위에 위치 해야합니다. 
----- 외부 파일을 읽는 건 여기서 설명하지 않습니다.
```

1. Resource/DataTable 디렉토리가 있다면 제거합니다. (데이터 꼬임 방지 및 최신의 데이터만 사용하도록)
2. 외부 DataTable 데이터를 Resource/DataTable  에 복사합니다.
3. Unity 에서 인지할 수 있도록 확장자를 .txt 로 변경합니다.

## 3. Make Build Option

씬, 빌드 타겟, 생성 파일 명, 생성 파일 경로를 세팅합니다.

1. 현재 에디터 BuildSetting 에 등록되어있는 Scene 들을 빌드 목록에 추가합니다.
2. 빌드 타겟을 설정합니다. (Android, iOS 만 지원)
3. 생성 경로 및 파일명을 설정합니다.


### 고려 사항 및 추가 정보.
- 패치를 위해서는 버전 관리가 필요하다.
- 외부 리소스의 경우 Application.persistentDataPath 에서 데이터를 가져올 수 잇다.