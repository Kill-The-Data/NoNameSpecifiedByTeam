using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class DebrisCollisionSound : MonoBehaviour
{
    public void Start()
    {
        var realSpace = GameObject.FindWithTag("DebrisField")?.GetComponent<NotifyAddChildren>();
        if (realSpace != null) realSpace.AddFakeChild(gameObject);
    }
    
}
