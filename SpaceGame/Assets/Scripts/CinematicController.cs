using UnityEngine;

public class CinematicController : MonoBehaviour
{
    [Header("---assign bar ui images---")]

    [SerializeField] private GameObject m_UpperBar = null;
    [SerializeField] private GameObject m_LowerBar = null;
    [Header("---should not be adjusted---")]
    [SerializeField] private float m_offset = 69;
    private float m_YPos = 0;
    [Header("---setup tween---")]
    [SerializeField] private float m_TweenSpeed = 0.75f;
    [SerializeField] private LeanTweenType m_TweenType = LeanTweenType.linear;

    [Header("Setup Externs")]
    [Tooltip("this is required to stop the player from moving in a cutscene")]
    [SerializeField] private PlayerController m_playerController;

    void Start()
    {
        Reset();
    }

    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown("1"))
            LerpIn();
        if (Input.GetKeyDown("2"))
            LerpOut();
#endif
    }

    //THIS DOES ONLY WORK FOR FIXED RESOLUTION 
    private void LerpIn()
    {
        m_playerController.InitCutscene();
        m_UpperBar.LeanMoveY(Screen.height - m_YPos, m_TweenSpeed).setEase(m_TweenType);
        m_LowerBar.LeanMoveY(m_YPos, m_TweenSpeed).setEase(m_TweenType);
    }
    private void LerpOut()
    {
        m_playerController.StopCutscene();
        m_UpperBar.LeanMoveY(Screen.height + m_YPos, m_TweenSpeed).setEase(m_TweenType);
        m_LowerBar.LeanMoveY(-m_YPos, m_TweenSpeed).setEase(m_TweenType);
    }
    private void Reset()
    {
        m_playerController.StopCutscene();
        m_YPos = m_offset * Screen.height / 1080;
      
        m_UpperBar.transform.position = new Vector3(Screen.width / 2, Screen.height + m_YPos, 0);
        m_LowerBar.transform.position = new Vector3(Screen.width / 2, -m_YPos, 0);
    }
}
