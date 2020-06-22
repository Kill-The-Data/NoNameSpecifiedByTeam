using System.Collections;
using System.Collections.Generic;
using EventHandler = SpaceGame.EventHandler;
using UnityEngine;
[RequireComponent(typeof(BuoyFillUp))]
public class FuelDrop : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset = Vector3.zero;
    [SerializeField] private GameObject m_fuelPrefab = null;

    private bool m_fuelDroppedThisRound = false;

    private void OnEnable()
    {
        EventHandler.Instance.StationFilled += StationFilled;
        EventHandler.Instance.TutorialStart += Reset;
    }
    private void onDisable()
    {
        EventHandler.Instance.StationFilled -= StationFilled;
        EventHandler.Instance.TutorialStart -= Reset;
    }
    private void Reset()
    {
        m_fuelDroppedThisRound = false;
    }
    public void StationFilled()
    {
        //get fill component, check if it is full
        BuoyFillUp fill = GetComponent<BuoyFillUp>();
        if (fill)
        {
            if (fill.GetState() == BuoyFillUp.BuoyCargoState.FULL)
                SpawnFuel();
        }
    }
    private void SpawnFuel()
    {
        if (m_fuelDroppedThisRound) return;
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

            var anim = GetComponent<Animator>();
            if (anim)
            {
                anim.SetTrigger("playDropOff");
            }

            m_fuelDroppedThisRound = true;
        }
    }
}
