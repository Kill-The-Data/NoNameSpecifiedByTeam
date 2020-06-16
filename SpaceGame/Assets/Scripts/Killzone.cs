using UnityEditor;
using UnityEngine;

public class Killzone : MonoBehaviour //Not to be confused with a calzone
{
    [SerializeField] private float m_killRadius;
    [SerializeField] private PlayerHealth m_victim;

    //Big number ... make sure it is sufficiently big
    private const int LIKE_A_LOT = 10000000;
    
    
    public void KillAllTheThings()
    {
        if (Vector3.Distance(m_victim.transform.position, transform.position) > m_killRadius)
        {
            //apply excruciating pain
            m_victim.TakeDamage(LIKE_A_LOT);
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position,Vector3.forward,m_killRadius);
    }
#endif

    public void Update()
    {
        KillAllTheThings();
    }
}
