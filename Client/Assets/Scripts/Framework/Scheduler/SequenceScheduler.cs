using System.Linq;
using Framework.Scheduler.Base;

namespace Framework.Scheduler
{
    public class SequenceScheduler : Base.Scheduler
    {
        protected Task m_current_task = null;

        public override void Update()
        {
            m_current_task = m_task_list.FirstOrDefault();
            if (m_current_task == null)
                return;

            m_current_task.Update();

            if (m_current_task.IsComplete())
            {
                m_current_task.OnComplete();

                m_task_list.Remove(m_current_task);
                m_current_task = null;
            }
        }
    }
}