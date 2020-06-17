using UnityEngine;
using TMPro;

[RequireComponent(typeof(EventHandler))]
public class WinCondition : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private IngameState m_state = null;

    [SerializeField] private int m_BuoysAmount = 0;
    [SerializeField] private int m_currentlyFilled = 0;
    [SerializeField] private bool m_winWithBuoysFilled = false;
    void Start()
    {
        Init();
    }

    private void StationFilled()
    {
        UpdateCount();
    }
    //listen to events
    private void Init()
    {
        EventHandler e = GetComponent<EventHandler>();
        e.GameStart += Begin;
        e.StationFilled += UpdateCount;
        e.GameFinished += Finish;
    }
    //reset vars & text
    private void Begin()
    {
        GameObject[] buoys = GameObject.FindGameObjectsWithTag("Station");
        m_BuoysAmount = buoys.Length;
        UpdateCount();
        UpdateText();
    }
    //get filled stations, update & check if game finished
    private void UpdateCount()
    {
        if (!m_winWithBuoysFilled) return;
        m_currentlyFilled = PlayerPrefs.GetInt("buoysFilled");
        UpdateText();
        if (m_currentlyFilled >= m_BuoysAmount)
        {
            m_state?.GameFinished();
        }
    }

    private void UpdateText()
    {
        m_text.gameObject.SetActive(true);
        string textDisplay = "FILLED :" +  m_currentlyFilled.ToString() +" / " +  m_BuoysAmount.ToString();
        m_text?.SetText(textDisplay);
    }

    private void Finish()
    {
        m_text.gameObject.SetActive(false);
        m_text.SetText("FILLED : 0 / 0");
    }
    
}
