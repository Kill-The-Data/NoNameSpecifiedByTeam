using UnityEngine;
[RequireComponent(typeof(NPCController))]
public class CamWayPointNavigator : WaypointNavigator
{
    private bool m_move = false;
    [SerializeField] private CamScroll m_camScroll;
    [SerializeField] private SmartCamZoom m_camZoom;


    protected override void Update()
    {
        //= is + on german keyboard layout
#if UNITY_EDITOR
        if (Input.GetKeyDown("3")) StartRide();
#endif
        if (m_move)
        {
            Move();
        }
    }
    //disable cam controls scripts && set bool true
    public void StartRide()
    {
        m_current = m_start;
        m_camScroll.enabled = false;
        m_camZoom.enabled = false;
        m_move = true;
    }
    //disable bool && set cam scripts active again
    public void EndRide()
    {
        m_camScroll.enabled = true;
        m_camZoom.enabled = true;
        m_move = false;
    }
    protected override void ReachedEnd()
    {
        EndRide();
    }
}
