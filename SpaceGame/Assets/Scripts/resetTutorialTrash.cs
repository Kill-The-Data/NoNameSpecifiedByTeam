using UnityEngine;
public class resetTutorialTrash : AReset
{

    private Vector3 m_originPos = Vector3.zero;
    [SerializeField] private GameObject m_trash =null;
    public override void Reset()
    {
        m_trash.SetActive(true);
        m_trash.transform.position = m_originPos;
    }

    void Awake()
    {
        m_originPos = m_trash.transform.position;

    }

}
