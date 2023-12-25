using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build Android")]
    public static void AndroidBuild()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        // 씬 추가
        List<string> scenes = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            scenes.Add(scene.path);
        }
        options.scenes = scenes.ToArray();
        Debug.Log("Included scene : " + scenes);

        // 타겟 경로(빌드 결과물이 여기 생성됨)
        options.locationPathName = "Build/Android";
        
        // 설정이 필요없을수 있다. 테스트 필요.
        // 빌드 타겟
        options.target = BuildTarget.Android;

        // 빌드
        BuildPipeline.BuildPlayer(options);
    }

    [MenuItem("Build/Build Aos")]
    public static void iOSBuild()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        // 씬 추가
        List<string> scenes = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            scenes.Add(scene.path);
        }
        options.scenes = scenes.ToArray();
        Debug.Log("Included scene : " + scenes);

        // 타겟 경로(빌드 결과물이 여기 생성됨)
        options.locationPathName = "Build/iOS";

        // 설정이 필요없을수 있다. 테스트 필요.
        // 빌드 타겟
        options.target = BuildTarget.iOS;
        // 빌드
        BuildPipeline.BuildPlayer(options);
    }
}