using System;
using UnityEngine;

public class GameMain : Framework.TMonoSingleton<GameMain>
{
 	static public bool Started { get; protected set; }

	protected override void Awake()
	{
		if (!PreAwake())
			return;

		Started = true;
		StaticInitialize();

        PostAwake();
	}

    void OnApplicationFocus(bool focus)
    {
        //Debug.LogFormat("[GameMain] OnApplicationFocus = {0}", focus.ToString());
    }

    void OnApplicationPause(bool pause)
    {
        //Debug.LogFormat("[GameMain] OnApplicationPause = {0}", pause.ToString());
    }

    void Update()
	{
        if (false == Started)
            return;

        UpdateNetworkGroup();
        UpdateSystemGroup();
	}

    private void UpdateNetworkGroup()
    {
        
    }

    private void UpdateSystemGroup()
    {

    }

    private void StaticInitialize()
    {
        InitializeSystemGroup();
        InitializeNetworkGroup(); 
    }

    private void InitializeSystemGroup()
    {
        GameTaskManager.Instance.StartGameTask();
    }

    private void InitializeNetworkGroup()
    {

    }
}
