using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimationController : MonoBehaviour
{
    public enum AnimationStates
    {
        PLAY_ON_START,
        TRASH_PICKUP,
        BUOY_ANIMATION,
        FUEL_DROP,
        MARS_ANIMATION,
        LID,
        FLAG,
        DAD,
        MARS_ENDING,
        RESTART,
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
    }
    public List<AnimationObject> animationObjects = new List<AnimationObject>(2);

    // Start is called before the first frame update
    void Start()
    {
        StartAnimation();
    }
    private void StartAnimation() 
    {
        foreach (AnimationObject currentObject in animationObjects)
        {

            if (currentObject.State == AnimationStates.PLAY_ON_START)
            {
                ActivateAnimation(currentObject);
            }
        }
    }


    public void TriggerAnimation(AnimationStates animationToTrigger)
    {
        if (animationToTrigger == AnimationStates.RESTART) 
        {
            Restart();
            return;
        }


       // Debug.Log("triggering next animation : " + animationToTrigger);
        foreach (AnimationObject a in animationObjects)
        {
            //make sure to only trigger properly setup animations
            if (a.State == AnimationStates.PLAY_ON_START || a.State==AnimationStates.NONE) continue;
            if (a.State == animationToTrigger)
            {
                ActivateAnimation(a);
            }
        }

    }

    private void Restart()
    {
       // Debug.Log("Restarting : ");
        StartAnimation();
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