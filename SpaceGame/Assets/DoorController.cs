using System;
using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    //prefabs to instantiate 
    [Header("---Door prefabs---")]
    [SerializeField] private GameObject m_firstDoorPrefab = null;
    [SerializeField] private GameObject m_secondDoorPrefab = null;
    //obejcts to store instantiated prefabs in 
    private GameObject m_FirstDoor = null;
    private GameObject m_SecondDoor = null;
    //tween stuff
    [Header("---Setup Tweening---")]
    [SerializeField] private float m_moveDistance = 25.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private LeanTweenType m_TweenType = LeanTweenType.easeOutExpo;
    //index to destroy the right door
    private int doorIndex = 0;

    //gets called by tutorial state on buoy filled,
    //opens door & increases index for destroying the right door
    public void OpenFirstDoor()
    {
        if (m_FirstDoor)
        {
            OpenDoor(m_FirstDoor.transform);
            doorIndex++;
        }
    }
    //gets called by tutorial state on tutorial finished,
    //opens door & increases index for destroying the right door
    public void OpenSecondDoor()
    {
        if (m_SecondDoor)
        {
            OpenDoor(m_SecondDoor.transform);
            doorIndex++;
        }
    }
    //"opens" door => moves it, fades it out , destroys it on complete
    //pls make sure that door objects are positioned properly
    private void OpenDoor(Transform targetTransform)
    {
        foreach (Transform child in targetTransform)
        {
            //decide on the move direction
            int dir = 1;
            if (child.transform.localPosition.y < 0) dir = -1;

            //move Object
            child.LeanMoveLocalY(child.transform.localPosition.y + m_moveDistance * dir, moveSpeed).setEase(m_TweenType).setOnComplete(DestroyDoor);

            //Fade alpha
            Renderer r = child.GetComponent<Renderer>();
            LeanTween.value(child.gameObject, 1, 0, moveSpeed).setEase(m_TweenType).setOnUpdate((float val) =>
            {
                r.material.SetFloat("ALPHA", val);
            });
        }
    }
    //destroys doors incrementally, depending on the index
    private void DestroyDoor()
    {
        if (doorIndex == 1 && m_FirstDoor)
        {
            Destroy(m_FirstDoor);
            m_FirstDoor = null;
        }
        else if (doorIndex == 2 && m_SecondDoor)
        {
            Destroy(m_SecondDoor);
            m_SecondDoor = null;
        }
    }
    //reset index && instantiate doors
    public void InitDoors()
    {
        doorIndex = 0;
        if (m_firstDoorPrefab)
            m_FirstDoor = Instantiate(m_firstDoorPrefab);
        if (m_secondDoorPrefab)
            m_SecondDoor = Instantiate(m_secondDoorPrefab);
    }
}
