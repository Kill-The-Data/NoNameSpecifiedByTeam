﻿using System;
using System.Collections;
using UnityEngine;
using EventHandler = SpaceGame.EventHandler;
public class TransitionOsts : MonoBehaviour
{
    [SerializeField] private OstPlayer m_ingame;
    [SerializeField] private OstPlayer m_menu;

    private void Start()
    {
        EventHandler.Instance.TutorialStart += MenuToIngame;
        EventHandler.Instance.GameFinished += IngameToMenu;
    }


    private void MenuToIngame()
    {
        m_ingame.StartSong();
        
        
        StartCoroutine(FadeVolume(0, m_menu.Volume, m_ingame,m_menu,true));
        StartCoroutine(FadeVolume(m_menu.Volume, 0, m_menu,m_ingame,false));
    }

    private void IngameToMenu()
    {
        m_menu.StartSong();
        
        StartCoroutine(FadeVolume(0, m_ingame.Volume, m_menu,m_ingame,true));
        StartCoroutine(FadeVolume(m_ingame.Volume, 0, m_ingame,m_menu,false));
    }

    IEnumerator FadeVolume(float current, float target,OstPlayer player,OstPlayer foreign,bool stopOther)
    {
        float time = 0;
        float value = current;
        
        while (Mathf.Abs(value - target) > 0.001f)
        {
            time += Time.deltaTime;
            value = Mathf.Lerp(current, target, time);
            player.Volume = value;
            yield return new WaitForFixedUpdate();
        }
        if(stopOther)
            foreign.StopSong();
    }
    
    
}
