using System.Collections.Generic;
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

    [SerializeField] private List<GameObject> m_stuffToDisable;
    
    void Start()
    {
        Reset();
    }

    void Update()
    {
        #if UNITY_EDITOR
            if (Input.GetKeyDown("1"))
                LowerBars();
            if (Input.GetKeyDown("2"))
                RaiseBars();
        #endif
    }

    //THIS DOES ONLY WORK FOR FIXED RESOLUTION 
    public void LowerBars()
    {
        m_stuffToDisable.ForEach(x => x.SetActive(false));
        m_playerController.BeginCutscene();
        m_UpperBar.LeanMoveY(Screen.height - m_YPos, m_TweenSpeed).setEase(m_TweenType);
        m_LowerBar.LeanMoveY(m_YPos, m_TweenSpeed).setEase(m_TweenType);
    }
    public void RaiseBars()
    {
        m_stuffToDisable.ForEach(x => x.SetActive(true));
        m_playerController.FinishCutscene();
        m_UpperBar.LeanMoveY(Screen.height + m_YPos, m_TweenSpeed).setEase(m_TweenType);
        m_LowerBar.LeanMoveY(-m_YPos, m_TweenSpeed).setEase(m_TweenType);
    }
    public void Reset(bool restoreUI = false)
    {
        m_playerController.FinishCutscene();
        if(restoreUI)
            m_stuffToDisable.ForEach(x => x.SetActive(true));
        m_YPos = m_offset * Screen.height / 1080;
      
        m_UpperBar.transform.position = new Vector3(Screen.width / 2, Screen.height + m_YPos, 0);
        m_LowerBar.transform.position = new Vector3(Screen.width / 2, -m_YPos, 0);
    }
}
