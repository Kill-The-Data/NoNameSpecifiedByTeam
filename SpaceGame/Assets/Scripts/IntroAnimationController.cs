using System;
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
    public List<AnimationObject> animationObjects = new List<AnimationObject>(2);

    void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        Restart();
    }
    //set handler for singleton
    private void Init()
    {
        Instance = this;
    }
    private void StartAnimation() 
    {
        //play all on start animations & deactivate all other animations
        foreach (AnimationObject currentObject in animationObjects)
        {
            if (currentObject.State == AnimationStates.PLAY_ON_START)
            {
                ActivateAnimation(currentObject);
            }
            else 
            {
                DeactivateAnimation(currentObject);
            }
        }
    }

    private void DeactivateAnimation(AnimationObject animationObject) 
    {
        //reset trigger 
        string TriggerString;
        if (animationObject.Trigger.Length == 0) TriggerString = "TriggerAnimation";
        else TriggerString = animationObject.Trigger;
        animationObject.animator.ResetTrigger(TriggerString);

        //reset to await state
        animationObject.animator.Play("AwaitState", 0);
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