using System.Collections.Generic;

namespace Framework.Scheduler.Base
{
    public class Scheduler
    {
        protected List<Task> m_task_list = new List<Task>();

        public int GetTaskCount()
        {
            return m_task_list.Count;
        }

        public virtual void AddTask(Task in_task)
        {
            if (in_task == null)
                return;

            in_task.OnAddTask();

            m_task_list.Add(in_task);
        }

        public virtual bool HasTask<T>() where T : Task
        {
            foreach (var task in m_task_list)
            {
                if (task is T)
                    return true;
            }

            return false;
        }

        public virtual void Update()
        {
            foreach (var task in m_task_list)
            {
                task.Update();

                if (task.IsComplete())
                {
                    task.OnComplete();
                }
            }

            m_task_list.RemoveAll(x => x.IsComplete());
        }

        public virtual void Release()
        {
            m_task_list.Clear();
        }
    }
}