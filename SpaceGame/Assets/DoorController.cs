using System;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject m_firstDoorPrefab = null;
    [SerializeField] private GameObject m_secondDoorPrefab = null;

    [SerializeField] private GameObject m_FirstDoor = null;
    [SerializeField] private GameObject m_SecondDoor = null;
    [SerializeField] private float m_moveDistance = 25.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private LeanTweenType m_TweenType = LeanTweenType.easeOutExpo;

    private int doorIndex = 0;
    public void OpenFirstDoor()
    {
        if (m_FirstDoor)
        { 
            OpenDoor(m_FirstDoor.transform);
            doorIndex++;
        }
    }
    public void OpenSecondDoor()
    {
        if (m_SecondDoor)
        { 
            OpenDoor(m_SecondDoor.transform);
            doorIndex++;
        }
    }
    //decide which object is above or below center of door
    //pls make sure that door objects are positioned properly
    private void OpenDoor(Transform targetTransform)
    {
        foreach (Transform child in targetTransform)
        {
            int dir = 1;
            if (child.transform.localPosition.y < 0) dir = -1;

            child.LeanMoveLocalY(child.transform.localPosition.y + m_moveDistance * dir, moveSpeed).setEase(m_TweenType).setOnComplete(DestroyDoor);

            //Fade alpha
            LeanTween.value(child.gameObject, 1, 0, moveSpeed).setEase(m_TweenType).setOnUpdate((float val) =>
            {
                Renderer r = child.GetComponent<Renderer>();

                r.material.SetFloat("ALPHA", val);
            });
        }
    }
    private void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
    private void DestroyDoor()
    {
        if (doorIndex == 1 && m_FirstDoor)
        {
            Destroy(m_FirstDoor);
            m_FirstDoor = null;
        }
        else if(doorIndex ==2 && m_SecondDoor)
        {
            Destroy(m_SecondDoor);
            m_FirstDoor = null;
        }
    }
    public void InitDoors()
    {
        doorIndex = 0;
        if (m_firstDoorPrefab)
            m_FirstDoor = Instantiate(m_firstDoorPrefab);
        if (m_secondDoorPrefab)
            m_SecondDoor = Instantiate(m_secondDoorPrefab);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
            OpenFirstDoor();
    }
}
