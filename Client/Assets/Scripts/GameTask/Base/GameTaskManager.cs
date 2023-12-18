using System;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Framework.Scheduler;
using Framework.Scheduler.Base;

class GameTaskManager : TMonoSingleton<GameTaskManager>
{
    private SequenceScheduler m_sequence_scheduler = new SequenceScheduler();

    private void Update()
    {
        m_sequence_scheduler.Update();
    }

    public void StartGameTask()
    {
        m_sequence_scheduler.AddTask(new GameTask_GameLogin());
    }
}