using System.Linq;
using Framework.Scheduler.Base;

namespace Framework.Scheduler
{
    // [TODO]
    // 'System.Threading.Tasks.Task' 얘랑 이름이 겹쳐서 DTask로 이름 수정
    using DTask = Framework.Scheduler.Base.Task;
    public class SequenceScheduler : Base.Scheduler
    {
        protected DTask m_current_task = null;

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