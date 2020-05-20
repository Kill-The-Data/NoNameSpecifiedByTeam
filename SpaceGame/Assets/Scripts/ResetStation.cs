using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStation : AReset
{


    public override void Reset()
    {
        GetComponent<BuoyFillUp>().Init();
    }
}
