using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchConfig : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WebConfigHandler.FetchConfig());
    }
    
}
