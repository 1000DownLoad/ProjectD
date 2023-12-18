using System;
using System.Collections.Generic;
using UnityEngine;
using Framework;

class PlayerController : MonoBehaviour
{
    private PlayerObject m_player_obj = null;

    private void Awake()
    {
        var player_obj = GameObject.FindWithTag("Player");
        if(player_obj != null)
        {
            m_player_obj = player_obj.GetComponent<PlayerObject>();
        }
    }

    private void Update()
    {
        if (m_player_obj == null)
            return;

        // 키보드 조작
        var key_dir = new Vector3(0f, 0f, 0f);
        bool is_attack = false;
        bool is_death = false;
        if (Input.GetKey(KeyCode.W))
        {
            key_dir.y += 1f;
        }

        if(Input.GetKey(KeyCode.A))
        {
            key_dir.x += -1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            key_dir.y += -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            key_dir.x += 1f;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            is_attack = true;
        }

        if (Input.GetKey(KeyCode.F))
        {
            is_death = true;
        }

        m_player_obj.transform.Translate(new Vector3(key_dir.x * 0.01f * 10, key_dir.y * 0.01f * 10, 0f));

        if(is_attack)
        {
            //m_player_obj.SetTimeScaleByTrack(m_player_obj.GetTrackIndex(SpineState.Attack1), 2f);

            if (m_player_obj.IsCompleteAnimationByTrack(m_player_obj.GetTrackIndex(SpineState.Attack1)))
                m_player_obj.SetState(SpineState.Attack1);
        }

        if(is_death)
        {
            m_player_obj.AllCompleteTack();
            m_player_obj.SetState(SpineState.Death);
        }
    }
}