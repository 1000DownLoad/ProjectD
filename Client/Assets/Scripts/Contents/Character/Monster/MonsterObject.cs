using System;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

class MonsterObject : SpineObject
{
    private PlayerObject m_target_player_obj = null;

    private void Awake()
    {
        var player_obj = GameObject.FindWithTag("Player");
        if (player_obj != null)
        {
            m_target_player_obj = player_obj.GetComponent<PlayerObject>();
        }
    }

    private void Update()
    {
        if (m_target_player_obj != null)
        {
            bool is_attack = false;
            bool is_death = false;

            var dir = m_target_player_obj.transform.position - this.transform.position;
            var dir_magnitude = dir.magnitude;
            dir.Normalize();

            if (dir.x < 0 || dir.x < 0)
            {
                SetLookDir(false);
            }
            else if (dir.x > 0 || dir.x > 0)
            {
                SetLookDir(true);
            }

            if (dir_magnitude > 7 || dir_magnitude > 7)
            {
                if (GetState() != SpineState.Death)
                {
                    SetState(SpineState.Run);
                    this.transform.Translate(new Vector3(dir.x * 0.01f * 5, dir.y * 0.01f * 5, 0f));
                }
            }
            else if (dir_magnitude < 7 || dir_magnitude < 7)
            {
                if (GetState() != SpineState.Death)
                {
                    SetState(SpineState.Idle);
                    is_attack = true;
                }
            }

            if (is_attack)
            {
                //m_player_obj.SetTimeScaleByTrack(m_player_obj.GetTrackIndex(SpineState.Attack1), 2f);

                if (IsCompleteAnimationByTrack(GetTrackIndex(SpineState.Attack1)))
                    SetState(SpineState.Attack1);
            }

            if (is_death)
            {
                AllCompleteTack();
                SetState(SpineState.Death);
            }
        }
    }

    public override string StateToString(SpineState in_state)
    {
        switch (in_state)
        {
            case SpineState.Idle: return "Idle";
            case SpineState.Run: return "Run";
            case SpineState.DamageTaken: return "Damage taken";
            case SpineState.Attack1: return "Attack";
            case SpineState.Attack2: return "Attack2";
            case SpineState.Death: return "Death";
            case SpineState.Jump: return "Gap closer";
            case SpineState.Happiness: return "Happiness";
            case SpineState.Sadness: return "Sadness";
        }

        return String.Empty;
    }

    public override bool IsLoopState(SpineState in_state)
    {
        switch (in_state)
        {
            case SpineState.Idle: return true;
            case SpineState.Run: return true;
            case SpineState.Happiness: return true;
            case SpineState.Sadness: return true;
        }

        return false;
    }

    public override int GetTrackIndex(SpineState in_state)
    {
        switch (in_state)
        {
            case SpineState.Idle: return (int)TrackIndexState.Default;
            case SpineState.Run: return (int)TrackIndexState.Default;
            case SpineState.Attack1: return (int)TrackIndexState.ActionAndDefault;
            case SpineState.Attack2: return (int)TrackIndexState.ActionAndDefault;
            case SpineState.Death: return (int)TrackIndexState.Action;

            default: return (int)TrackIndexState.Action;
        }
    }
}
