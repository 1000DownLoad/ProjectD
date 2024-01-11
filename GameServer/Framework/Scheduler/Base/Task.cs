

namespace Framework.Scheduler.Base
{
    public enum ETaskState
    {
        Process,
        Failed,
        Success,
    }

    public abstract class Task
    {
        private     bool        m_is_init       = false;
        protected   ETaskState  m_task_state    = ETaskState.Process;

        protected void Complete(ETaskState in_task_state)
        {
            m_task_state = in_task_state;
        }

        public void Update()
        {
            if (m_is_init == false)
            {
                m_is_init = true;
                OnAwake();
            }

            OnUpdate();
        }

        public virtual void OnAddTask()
        {

        }

        public virtual void OnAwake()
        {

        }

        public virtual void OnUpdate()
        {

        }


        public virtual void OnComplete()
        {

        }

        public bool IsComplete()
        {
            return m_task_state == ETaskState.Success || m_task_state == ETaskState.Failed;
        }

        public bool IsSuccess()
        {
            return m_task_state == ETaskState.Success;
        }

        public bool IsFailed()
        {
            return m_task_state == ETaskState.Failed;
        }

        public bool IsProcess()
        {
            return m_task_state == ETaskState.Process;
        }

        public ETaskState GetState()
        {
            return m_task_state;
        }
    }
}
