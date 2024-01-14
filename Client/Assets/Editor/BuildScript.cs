using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;
using System.IO;

// 젠킨스에서 사용되는 빌드 스크립트입니다.
public class BuildScript
{
    [MenuItem("Build/Build Android")]
    public static void AndroidBuild()
    {
        Debug.Log("Android Build Start");

        // 데이터 폴더를 복사합니다.
        CopyDataTableAndChnageExtension();

        // 애플리 케이션 프로세스와 Android 에서 자료 추출 방지
        // 우선 Debug key 를 사용하도록 세팅해둔다. 좀더 자세한 확인필요.
        // https://developer.android.com/training/articles/keystore?hl=ko
        PlayerSettings.Android.useCustomKeystore = false;

        // 빌드 옵션 설정.
        // 디렉토리 및 프로젝트 이름을 외부에서 가져오도록 변경하는것을 고려해야됨.
        var build_option = MakeBuildOption(BuildTarget.Android, "Build", "ProjectD.apk");

        // 빌드
        BuildReport report = BuildPipeline.BuildPlayer(build_option);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded : " + summary.totalSize + " bytes");
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    [MenuItem("Build/Build Aos")]
    public static void iOSBuild()
    {
        // 데이터 폴더를 복사합니다.
        CopyDataTableAndChnageExtension();


        // 빌드 옵션 설정.
        // 디렉토리 및 프로젝트 이름을 외부에서 가져오도록 변경하는것을 고려해야됨.
        var build_option = MakeBuildOption(BuildTarget.iOS, "Build", "ProjectD.ios");

        // 빌드
        BuildReport report = BuildPipeline.BuildPlayer(build_option);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded : " + summary.totalSize + " bytes");
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    // 빌드 옵션을 설정한다.
    //  1. 씬 추가.
    //  2. 빌드 타겟 설정.
    //  3. 빌드 결과물 경로 및 이름 설정.
    private static BuildPlayerOptions MakeBuildOption(BuildTarget in_build_target, string in_output_path, string in_file_name)
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
        Debug.Log("MakeBuildOption - Included scene : " + scenes);


        // 빌드 타겟
        options.target = in_build_target;
        Debug.Log("MakeBuildOption - target : " + in_build_target);

        // 타겟 경로(빌드 결과물이 여기 생성됨)
        var location_path = Path.Combine(in_output_path, in_file_name);
        options.locationPathName = location_path;
        Debug.Log("MakeBuildOption - output info : " + location_path);

        return options;
    }

    // 빌드 파일에 DataTable 을 포함시키기위하여 추가된 함수.
    // 동작
    //  1. Resource/DataTables 경로에 엑셀을 복사합니다.
    //     ㄴ 만약 Resource/DataTables 폴더가 이미 존재하는 경우 삭제 후 복사를 진행합니다.
    //  2. 엑셀은 Unity 에서 인식을 하지 못하기 대문에 .text 파일로 변경합니다.
    private static void CopyDataTableAndChnageExtension()
    {
        Debug.Log("Copy DataTables start");

        string copy_path = Path.Combine("Assets", "Resources", "DataTable");
        string source_path = Path.Combine("..", "DataTable");
        string target_extension = ".xlsx";
        string chagen_extension = ".text";

        // 이전에 복사된 데이터 디렉토리 삭제.
        // 데이터가 꼬이는 문제를 방지하기 위함.
        if (Directory.Exists(copy_path))
        {
            Directory.Delete(copy_path, true /* recursive */);
            Debug.Log("Delet old DataTable directory : " + copy_path);
        }

        CopyDirectory(source_path, copy_path);

        Debug.Log("Copy DataTable finish");



        Debug.Log("Change extension start");

        // 확장자 변경.
        ChangeFileExtensions(copy_path, target_extension, chagen_extension);

        Debug.Log("Change extension finish");
    }


    public static void CopyDirectory(string in_source_path, string in_target_path)
    {
        // 대상 디렉토리가 존재하지 않으면 생성합니다.
        Directory.CreateDirectory(in_target_path);

        // 현재 경로의 파일과 폴더 목록을 가져옵니다.
        string[] entries = Directory.GetFileSystemEntries(in_source_path);

        foreach (string entry in entries)
        {
            string targetEntryPath = Path.Combine(in_target_path, Path.GetFileName(entry));

            if (Directory.Exists(entry))
            {
                // 폴더인 경우 재귀적으로 복사합니다.
                CopyDirectory(entry, targetEntryPath);
            }
            else
            {
                // 파일인 경우 복사합니다.
                File.Copy(entry, targetEntryPath, true); // true는 기존 파일을 덮어쓰도록 합니다.
            }
        }
    }

    public static void ChangeFileExtensions(string in_source_path, string in_target_extension, string in_change_extension)
    {
        // 현재 경로의 파일과 폴더 목록을 가져옵니다.
        string[] entries = Directory.GetFileSystemEntries(in_source_path);

        foreach (string entry in entries)
        {
            string targetEntryPath = Path.Combine(in_source_path, Path.GetFileName(entry));

            if (Directory.Exists(entry))
            {
                // 폴더인 경우 재귀적으로 경로를 변경해준다.
                ChangeFileExtensions(targetEntryPath, in_target_extension, in_change_extension);
            }
            else
            {
                // 파일인 경우 target 확장자인지 확인.
                string extension = Path.GetExtension(entry);
                if (extension.Equals(in_target_extension))
                {
                    // 확장자가 변경된 경로 생성.
                    string changed_path = Path.ChangeExtension(entry, in_change_extension);

                    // 파일 변경.
                    File.Move(entry, changed_path);
                }
            }
        }
    }
}