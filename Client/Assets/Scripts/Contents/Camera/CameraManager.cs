using System;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType
{
    // �ʿ信 ���� ����.
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

            // ī�޶� ���� ��ġ�� ���Դϴ�.
            camera.transform.parent = this.transform;

            // Ȯ�� �Լ� �߰��� �ʿ��մϴ�.
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

        // �� ī�޶� ������Ʈ ����
        GameObject game_obj = new GameObject(name);
        var camera = game_obj.AddComponent<Camera>();
        if (camera == null) 
        {
            Debug.LogError($"Create Camera Fail : {name}");
            return null;
        }

        // BackGround Color ����
        // Ŭ���� �� �÷�.
        camera.backgroundColor = new Color(0, 0, 0);

        // Culling Mask ����
        camera.cullingMask = GetCullingMask(in_camera_type);

        // Clear Flags ����
        camera.clearFlags = GetClearFlags(in_camera_type);

        // Projection ����
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
