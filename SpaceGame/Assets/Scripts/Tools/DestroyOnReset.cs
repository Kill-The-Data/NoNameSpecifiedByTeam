using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnReset : AReset
{
    public override void Reset()
    {
        Destroy(gameObject);
    }
}
   
