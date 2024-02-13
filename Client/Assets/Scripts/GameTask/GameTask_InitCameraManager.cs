using Framework.Scheduler.Base;
using UnityEngine;

class GameTask_InitCameraManager : Task
{
    public override void OnAwake()
    {
        Debug.Log("GameTask_InitCameraManager OnAwake");

        CameraManager.Instance.Init();
    }

    public override void OnUpdate()
    {
        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {
        Debug.Log("GameTask_InitCameraManager OnComplete");
    }
}