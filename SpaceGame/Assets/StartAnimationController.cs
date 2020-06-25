using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animtor = null;
    private void OnEnable()
    {
        animtor?.gameObject.SetActive(true);
        animtor?.SetTrigger("TriggerAnimation");
    }
      
}
