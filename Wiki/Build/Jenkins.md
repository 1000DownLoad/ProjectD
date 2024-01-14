# 젠킨스

# 목표
1. git master 브랜치에 커밋을 감지.
2. 젠킨스를 통해 빌드 및 배포 진행
3. 빌드 파일은 깃 릴리즈를 통해 배포.

# 동작 이미지

![배포 구성도](https://github.com/1000DownLoad/ProjectD/assets/16574400/e7826291-0a98-4712-9e59-6ebab375eaa3)


# 빌드 환경
1. 맥 M1 머신
2. 젠킨스
	*  brew 를 통해 설치.
	* 포트는 9090
	* 문제 발생시 아래 명령어로 젠킨스 재실행
		* brew services restart jenkins
	* 설치 플러그인
		* Unity3d plugin
		* Discord Notifier (빌드 결과 전송용)
		* Githpub plugin
		* Post Build Task
3. 기타
	*  jq (brew 를 통해 설치)
		* curl 결과를 json 으로 편하게 가져오기 위해 사용.


# 상세 동작	
1. Github push 시 웹콜 전송.
2. 젠킨스 ProjectD 에서 웹콜의 브랜치를 체크 및 빌드 진행
	1. Build Step - Invoke Unity Editor
	2. 유니티의 Editor 스크립트 메소드를 실행시켜준다.
		```
		-quit -batchmode  -stackTraceLogType Full -projectPath '$WORKSPACE/Client' -executeMethod BuildScript.AndroidBuild -buildTarget Android -usedebug true
		```
	3.  빌드가 완료되면 Post build Task 를 통해 깃 릴리즈 등록
		1. 빌드 로그 텍스트의 *Build Finished, Result: Success.* 문자열 감지
		2. curl 을 통해 릴리즈 생성
		3. curl 을 통해 빌드 압축 파일 등록.
		```
		 #!/bin/bash
		zip /Users/baeseongjin/.jenkins/workspace/ProjectD/Client/Build/ProjectD.apk/ProjectD_APK.zip /Users/baeseongjin/.jenkins/workspace/ProjectD/Client/Build/ProjectD.apk/Project.apk

		RESPONSE=$(curl -L \
		    -X POST \
		    -H "Accept: application/vnd.github+json" \
		    -H "Authorization: token 토큰 정보" \
		    -H "X-GitHub-Api-Version: 2022-11-28" \
		    https://api.github.com/repos/1000DownLoad/ProjectD/releases \
		    -d "{\"tag_name\":\"v1.0.${BUILD_NUMBER}\",\"target_commitish\":\"main\",\"name\":\"v1.0.${BUILD_NUMBER}\",\"body\":\"Description of the release\",\"draft\":false,\"prerelease\":false,\"generate_release_notes\":false}")


		RELEASE_ID=$(echo $RESPONSE | /opt/homebrew/bin/jq '.id')

		curl -X POST \
		     -H "Authorization: token 토큰 정보" \
		     -H "Content-Type: application/octet-stream" \
		     --data-binary @"/Users/baeseongjin/.jenkins/workspace/ProjectD/Client/Build/ProjectD_APK.zip" \
		     "https://uploads.github.com/repos/1000DownLoad/ProjectD/releases/${RELEASE_ID}/assets?name=ProjectD_APK.zip"
		```
	4. 빌드 결과를 Discord Notifier 로 디스코드에 알림