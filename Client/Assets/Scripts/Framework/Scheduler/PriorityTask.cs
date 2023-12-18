using System;
using Framework.Scheduler.Base;

namespace Framework.Scheduler
{
    public abstract class PriorityTask : Task
    {
        protected PriorityTask()
        {
            m_create_time = DateTime.Now;
        }

        public DateTime m_create_time { get; protected set; }

        public abstract int Priority { get; set; }
    }
}
