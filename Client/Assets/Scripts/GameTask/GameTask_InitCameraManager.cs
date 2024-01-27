using Framework.Scheduler.Base;

class GameTask_InitCameraManager : Task
{
    public override void OnAddTask()
    {

    }

    public override void OnAwake()
    {
        CameraManager.Instance.Init();
    }

    public override void OnUpdate()
    {
        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {

    }
}