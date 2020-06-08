using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BuoyFillUp))]
public class FuelDrop : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset = Vector3.zero;
    [SerializeField] private GameObject m_fuelPrefab = null;
    private void Start()
    {
        AttachEvent();
    }
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
        }
    }
    private void AttachEvent()
    {
        EventHandler e = EventSingleton.Instance.EventHandler;
        if (e)
        {
            //listen to the station filled event
            e.StationFilled += StationFilled;
        }
    }

    public void StationFilled()
    {
        //get fill component, check if it is full
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
            //create empty parent object
            //tag it as reset & add reset component to ensure it gets destroyed before next play through
            GameObject parentObj = new GameObject();
            parentObj.tag = "Reset";
            parentObj.AddComponent<DestroyOnReset>();

            //instantiate fuel object, set parent and parent position
            var Obj = Instantiate(m_fuelPrefab);
            parentObj.transform.position = this.transform.position;
            Obj.transform.parent = parentObj.transform;
            Obj.transform.localPosition = Vector3.zero;

            //get animator component & trigger it
            Animator a = GetComponent<Animator>();
            a?.SetTrigger("playDropOff");
        }
    }
}
