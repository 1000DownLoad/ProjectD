using System;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType
{
    // 필요에 따라 수정.
    ClearCamera,
    GameCamera,
    UICamera,
    MAX
}

public class CameraManager : Framework.TMonoSingleton<CameraManager>
{
    private Dictionary<CameraType, Camera> m_managed_cameras = new Dictionary<CameraType, Camera>();

    public void Init()
    {
        for (int i = 0; i < (int)CameraType.MAX; ++i) 
        {
            var type = (CameraType)i;

            var camera = CreateCamear(type);
            if (camera == null)
            {
                Debug.LogError($"CameraManager create fail {type}");
                return;
            }

            // 카메라를 현재 위치로 붙입니다.
            camera.transform.parent = this.transform;

            // 확장 함수 추가가 필요합니다.
            camera.transform.localRotation = Quaternion.identity;
            camera.transform.localPosition = Vector3.zero;
            camera.transform.localScale = Vector3.one;
        }    
    }

    public Camera GetCamera(CameraType in_camera_type)
    {
        if (m_managed_cameras.ContainsKey(in_camera_type) == false)
        {
            Debug.LogError($"CameraManager GetCamera {in_camera_type.ToString()} not found");
            return null;
        }

        return m_managed_cameras[in_camera_type];
    }

    private Camera CreateCamear(CameraType in_camera_type) 
    {
        string name = in_camera_type.ToString();

        // 새 카메라 오브젝트 생성
        GameObject game_obj = new GameObject(name);
        var camera = game_obj.AddComponent<Camera>();
        if (camera == null) 
        {
            Debug.LogError($"Create Camera Fail : {name}");
            return null;
        }

        // BackGround Color 설정
        // 클리어 용 컬러.
        camera.backgroundColor = new Color(0, 0, 0);

        // Culling Mask 설정
        camera.cullingMask = GetCullingMask(in_camera_type);

        // Clear Flags 설정
        camera.clearFlags = GetClearFlags(in_camera_type);

        // Projection 설정
        camera.orthographic = GetOrthographic(in_camera_type);

        if (m_managed_cameras.ContainsKey(in_camera_type))
            Debug.LogError("CameraManager already created camera");
        else
            m_managed_cameras.Add(in_camera_type, camera);

        return camera;
    }

    private int GetCullingMask(CameraType in_camera_type) 
    {
        switch (in_camera_type) 
        {
            case CameraType.ClearCamera:
                return LayerMask.GetMask("Nothing");
            case CameraType.UICamera:
                return LayerMask.GetMask("UI");
            default:
                return LayerMask.GetMask("Default");
        }
    }

    private CameraClearFlags GetClearFlags(CameraType in_camera_type)
    {
        switch (in_camera_type) 
        {
            case CameraType.UICamera:
                return CameraClearFlags.Depth;
            case CameraType.ClearCamera:
                return CameraClearFlags.SolidColor;
            default:
                return CameraClearFlags.Nothing;
        }
    }

    private bool GetOrthographic(CameraType in_camera_type)
    {
        switch (in_camera_type)
        {
            default:
                return true;
        }
    }
}
