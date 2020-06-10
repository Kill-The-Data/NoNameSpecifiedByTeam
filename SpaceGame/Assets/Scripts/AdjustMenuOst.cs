using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AdjustMenuOst : MonoBehaviour
{
    [SerializeField] private GameObject GlobalSetup;
    
    
    void Start()
    {
        var slider = GetComponent<Slider>();
        OstPlayer[] players = GlobalSetup.GetComponents<OstPlayer>();

        var player = players.First(x => x.OstTrack == "menu-ost");

        slider.onValueChanged.AddListener(player.AdjustVolume);
    }
    
}
