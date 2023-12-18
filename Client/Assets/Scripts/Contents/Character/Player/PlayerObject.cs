using System;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

class PlayerObject : SpineObject
{
    public override string StateToString(SpineState in_state)
    {
        switch(in_state)
        {
            case SpineState.Idle           : return "Idle";
            case SpineState.Run            : return "Run";
            case SpineState.DamageTaken    : return "Damage taken";
            case SpineState.Attack1        : return "Attack";
            case SpineState.Attack2        : return "Attack2";
            case SpineState.Death          : return "Death";
            case SpineState.Jump           : return "Gap closer";
            case SpineState.Happiness      : return "Happiness";
            case SpineState.Sadness        : return "Sadness";
        }

        return String.Empty;
    }

    public override bool IsLoopState(SpineState in_state)
    {
        switch (in_state)
        {
            case SpineState.Idle        : return true;
            case SpineState.Run         : return true;
            case SpineState.Happiness   : return true;
            case SpineState.Sadness     : return true;
        }

        return false;
    }

    public override int GetTrackIndex(SpineState in_state)
    {
        switch (in_state)
        {
            case SpineState.Idle            : return (int)TrackIndexState.Default;
            case SpineState.Run             : return (int)TrackIndexState.Default;
            case SpineState.Attack1         : return (int)TrackIndexState.ActionAndDefault;
            case SpineState.Attack2         : return (int)TrackIndexState.ActionAndDefault;
            case SpineState.Death           : return (int)TrackIndexState.Action;

            default: return (int)TrackIndexState.Action;
        }
    }

    private void Update()
    {
        
    }
}
