# Sever 환경.
c# .Net core 3.1 을 사용한다.
- Unity 2021.3.24f 와 동일한 visual studio 버전을 사용한다.
- 해당 visual studio 에서 지원되는 버전은 .net 5.0, .net core 3.1 이 존재.
- .net 5.0 버전에서는  Concurrent Container 의 호환성 문제가 존재. (.net 6.0 이후부터 정식 지원함)
- 위와 같은 이유로 .net core 3.1 로 버전 채택


# 환경 설정 방법.
 - visual studio 2021.3.24 버전을 받는다.
 - .net core 3.1 을 다운로드 받는다.
	 - [https://dotnet.microsoft.com/ko-kr/download/dotnet/thank-you/sdk-3.1.426-windows-x64-installer](https://dotnet.microsoft.com/ko-kr/download/dotnet/thank-you/sdk-3.1.426-windows-x64-installer "https://dotnet.microsoft.com/ko-kr/download/dotnet/thank-you/sdk-3.1.426-windows-x64-installer")
	 - visual studio installer 에서 받는 방식보다 직접 다운로드 받는게 편해 링크 공유.
	 - visual studio installer 에서는 컨버트 툴을 사용할 수 없으며 별도의 환경이 세팅되는듯하다.

# .Net 마이그레이션 방법.

1. dotnet tool 에서 try-conver 다운로드.
```
dotnet tool install -g try-convert
```
	
2. try-conver 을 통해 현재 프로젝트의 csproj 파일 변경.
```
try-convert -p "C:\1000Download\ProjectD\GameServer\GameServer.csproj" -tfm netcoreapp3.1
```

3. 만약 지원이 불가능한 패키지가 있다면 실패하며 해당 패키지는 사용 여부를 체크 후 제거 또는 컨버팅.

4. 이후 서버 프로젝트를 실행 후 경고가 뜨는 패키지들을 Nuget 으로 업데이트 진행.
	- 만약 업데이트로 경고 해결이 안되면 새로운 패키지를 찾아 바꿔줘야 한다. 
	- 새로운 이름으로 변경하여 라이브러리를 제공할 가능성이 큼.
5.  정상 동작하는지 체크.
