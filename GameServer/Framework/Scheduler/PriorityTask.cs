using System;
using Framework.Scheduler.Base;

namespace Framework.Scheduler
{
    // [TODO]
    // 'System.Threading.Tasks.Task' 얘랑 이름이 겹쳐서 DTask로 이름 수정
    using DTask = Framework.Scheduler.Base.Task;
    public abstract class PriorityTask : DTask
    {
        protected PriorityTask()
        {
            m_create_time = DateTime.Now;
        }

        public DateTime m_create_time { get; protected set; }

        public abstract int Priority { get; set; }
    }
}
