using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuoyFillUp))]
public class ResetStation : AReset
{
    public override void Reset()
    {
        GetComponent<BuoyFillUp>().Init();
        GetComponent<AudioSource>().volume = 0;
    }
}
