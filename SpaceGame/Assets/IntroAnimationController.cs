using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimationController : MonoBehaviour
{
    public enum AnimationStates
    {
        PlayOnStart,
        TRASH_PICKUP,
        BUOY_ANIMATION,
        FUEL_DROP,
        NONE

    }


    public static IntroAnimationController Instance;


    void Awake()
    {
        Init();
    }
    //set handler for singleton
    private void Init()
    {
        Instance = this;
        IntroAnimationController.Instance = this;
    }
    public List<AnimationObject> animationObjects = new List<AnimationObject>(2);

    // Start is called before the first frame update
    void Start()
    {
        foreach (AnimationObject currentObject in animationObjects)
        {

            if (currentObject.State==AnimationStates.PlayOnStart)
            {
                ActivateAnimation(currentObject);
            }
        }
    }


    public void TriggerAnimation(AnimationStates animationToTrigger)
    {

        Debug.Log("triggering next animation : " + animationToTrigger);
        foreach (AnimationObject a in animationObjects)
        {
            if (a.State == AnimationStates.PlayOnStart ||a.State==AnimationStates.NONE) continue;
            if (a.State == animationToTrigger)
            {
                ActivateAnimation(a);
            }
        }

    }


    private void ActivateAnimation(AnimationObject a)
    {
        string TriggerString;

        if (a.Trigger.Length == 0) TriggerString = "TriggerAnimation";
        else TriggerString = a.Trigger;
        a.animator.SetTrigger(TriggerString);

    }
}
[Serializable]
public class AnimationObject
{
    public Animator animator;
    public string Trigger;
    public IntroAnimationController.AnimationStates State;
}

public static class AnimationStates
{
  

}