using UnityEngine;

public class CinematicController : MonoBehaviour
{

    [SerializeField] private GameObject m_UpperBar = null;
    [SerializeField] private GameObject m_LowerBar = null;

    [SerializeField] private float m_invisPos = 0;
    [SerializeField] private float m_visPos = 0;
    [SerializeField] private float m_TweenSpeed = 0.75f;
    [SerializeField] private LeanTweenType m_TweenType = LeanTweenType.linear;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        //if (Input.GetKeyDown("space"))
        //    LerpIn();
        //if (Input.GetKeyDown("="))
        //    LerpOut();
    }

    //THIS DOES ONLY WORK FOR FIXED RESOLUTION 
    private void LerpIn()
    {
        m_UpperBar.LeanMoveY(Screen.height - m_visPos, m_TweenSpeed).setEase(m_TweenType);
        m_LowerBar.LeanMoveY(m_visPos, m_TweenSpeed).setEase(m_TweenType);
    }
    private void LerpOut()
    {
        m_UpperBar.LeanMoveY(Screen.height + m_visPos, m_TweenSpeed).setEase(m_TweenType);
        m_LowerBar.LeanMoveY(-m_invisPos, m_TweenSpeed).setEase(m_TweenType);
    }
    private void Reset()
    {
        m_UpperBar.transform.position = new Vector3(Screen.width / 2, Screen.height + m_visPos, 0);
        m_LowerBar.transform.position = new Vector3(Screen.width / 2 - m_visPos, --m_invisPos, 0);
    }
}
