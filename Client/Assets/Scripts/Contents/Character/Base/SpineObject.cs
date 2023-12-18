using System;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using System.Collections;

enum SpineState
{
    Idle,
    Run,
    DamageTaken,
    Attack1,
    Attack2,
    Death,
    Jump,
    Stun,
    Happiness,
    Sadness,
}

enum TrackIndexState
{
    Default,
    Action,
    ActionAndDefault,
}

abstract class SpineObject : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation m_skeleton_animation;

    private SpineState m_cur_state = SpineState.Idle;

    private void Awake()
    {
        if(m_skeleton_animation != null)
        {
            m_skeleton_animation.AnimationState.Start += OnAnimationStart;
            m_skeleton_animation.AnimationState.Complete += OnAnimationComplete;
            m_skeleton_animation.AnimationState.End += OnAnimationEnd;
        }
    }

    private void Start()
    {
        PlayAnimation(0, SpineState.Idle, true);
    }

    abstract public string StateToString(SpineState in_state);
    abstract public bool IsLoopState(SpineState in_state);
    abstract public int GetTrackIndex(SpineState in_state);

    public void SetState(SpineState in_state)
    {
        if (m_cur_state != in_state)
        {
            m_cur_state = in_state;

            PlayAnimation(GetTrackIndex(m_cur_state), m_cur_state, IsLoopState(m_cur_state));
        }
    }

    public SpineState GetState()
    {
        return m_cur_state;
    }

    public void PlayAnimation(int in_track_index, SpineState in_state, bool in_is_loop)
    {
        if (m_skeleton_animation == null)
            return;

        var track_entry = m_skeleton_animation.AnimationState.SetAnimation(in_track_index, StateToString(in_state), in_is_loop);
        if(track_entry != null)
        {
            track_entry.MixDuration = 0.2f;
        }
    }

    public void SetLookDir(bool in_is_right)
    {
        if (m_skeleton_animation == null)
            return;

        if (in_is_right)
            m_skeleton_animation.skeleton.ScaleX = Mathf.Abs(m_skeleton_animation.skeleton.ScaleX);
        else
            m_skeleton_animation.skeleton.ScaleX = -Mathf.Abs(m_skeleton_animation.skeleton.ScaleX);
    }

    public bool IsCompleteAnimationByTrack(int in_track_index)
    {
        if (m_skeleton_animation == null)
            return true;

        var cur_anim = m_skeleton_animation.AnimationState.GetCurrent(in_track_index);
        if (cur_anim != null)
            return cur_anim.IsComplete;

        return true;
    }

    public void SetTimeScaleByTrack(int in_track_index, float in_time_scale)
    {
        if (m_skeleton_animation == null)
            return;

        var cur_anim = m_skeleton_animation.AnimationState.GetCurrent(in_track_index);
        if (cur_anim != null)
            cur_anim.TimeScale = in_time_scale;
    }

    virtual protected void OnAnimationStart(TrackEntry in_track_entry)
    {

    }

    virtual protected void OnAnimationComplete(TrackEntry in_track_entry)
    {
        if(in_track_entry.TrackIndex == (int)TrackIndexState.ActionAndDefault)
        {
            var cur_default_anim = m_skeleton_animation.AnimationState.GetCurrent((int)TrackIndexState.Default);
            if (cur_default_anim == null)
                return;

            var track_entry = m_skeleton_animation.AnimationState.SetAnimation(in_track_entry.TrackIndex, cur_default_anim.Animation, false);
            if (track_entry != null)
            {
                track_entry.MixDuration = cur_default_anim.MixDuration;
                StartCoroutine(OnDelayClearTrack(in_track_entry.TrackIndex, track_entry.MixDuration));
            }
        }
    }

    virtual protected void OnAnimationEnd(TrackEntry in_track_entry)
    {

    }

    public void ClearTrack(int in_track_index)
    {
        m_skeleton_animation.AnimationState.ClearTrack(in_track_index);
    }

    public void AllCompleteTack()
    {
        foreach(var track in m_skeleton_animation.AnimationState.Tracks)
        {
            if(track != null)
                track.TrackTime = track.AnimationEnd;
        }
    }

    IEnumerator OnDelayClearTrack(int in_track_index, float in_delay)
    {
        yield return new WaitForSeconds(in_delay);

        ClearTrack(in_track_index);
    }
}
