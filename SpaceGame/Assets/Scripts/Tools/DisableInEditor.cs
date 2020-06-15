using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInEditor : MonoBehaviour
{
    void Start()
    {
        #if UNITY_EDITOR
            gameObject.SetActive(false);
        #endif
    }
}
