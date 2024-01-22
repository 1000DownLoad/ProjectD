using Framework.Scheduler.Base;

namespace Framework.Scheduler
{
    // [TODO]
    // 'System.Threading.Tasks.Task' 얘랑 이름이 겹쳐서 DTask로 이름 수정
    using DTask = Framework.Scheduler.Base.Task;
    public class PriorityScheduler : SequenceScheduler
    {
        public override void AddTask(DTask task)
        {
            if (task == null)
                return;

            base.AddTask(task);

            SortTask();
        }

        private void SortTask()
        {
            m_task_list.Sort(SortCompare);
        }

        private int SortCompare<T>(T a, T b) where T : DTask
        {
            var first = a as PriorityTask;
            var sec = b as PriorityTask;

            if (first == null || sec == null) return 0;

            if (first.Priority > sec.Priority) return 1;
            if (first.Priority < sec.Priority) return -1;

            if (first.m_create_time > sec.m_create_time) return 1;
            if (first.m_create_time < sec.m_create_time) return -1;

            return 0;
        }
    }
}