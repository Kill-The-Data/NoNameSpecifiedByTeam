using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelDrop : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset = Vector3.zero;
    [SerializeField] private GameObject m_fuelPrefab = null;
    private void Start()
    {
        Debug.Log("Hello start");
        AttachEvent();
    }
    //private void OnDisable()
    //{
    //    DetachEvent();
    //}
    private void OnDestroy()
    {
        DetachEvent();
    }
    private void DetachEvent()
    {
        EventHandler e = EventSingleton.Instance?.EventHandler;
        if (e)
        {
            e.StationFilled -= StationFilled;
            Debug.Log("stopped listening");
        }
    }
    private void AttachEvent()
    {
        EventHandler e = EventSingleton.Instance.EventHandler;
        if (e)
        {
            e.StationFilled += StationFilled;
            Debug.Log("event listening");
        }
    }

    public void StationFilled()
    {
        Debug.Log("station filled");
        BuoyFillUp fill = GetComponent<BuoyFillUp>();
        if (fill)
        {
            if (fill.Full())
                SpawnFuel();
        }
    }
    private void SpawnFuel()
    {
        if (m_fuelPrefab)
        {
            GameObject parentObj = new GameObject();
            var Obj = Instantiate(m_fuelPrefab);
            parentObj.transform.position = this.transform.position;
            Obj.transform.parent=parentObj.transform;
            Obj.transform.localPosition=Vector3.zero;
            Animator a = GetComponent<Animator>();
            if(a)
                a.SetTrigger("playDropOff");
        }
    }
}
