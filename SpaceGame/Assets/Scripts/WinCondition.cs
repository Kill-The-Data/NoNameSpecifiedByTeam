using UnityEngine;
using TMPro;

[RequireComponent(typeof(EventHandler))]
public class WinCondition : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private IngameState m_state = null;

    [SerializeField] private int m_BuoysAmount = 0;
    [SerializeField] private int m_currentlyFilled = 0;
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
        e.GameStart += Reset;
        e.StationFilled += UpdateCount;
    }
    //reset vars & text
    private void Reset()
    {
        GameObject[] buoys = GameObject.FindGameObjectsWithTag("Station");
        m_BuoysAmount = buoys.Length;
        m_currentlyFilled = 0;
        m_text.gameObject.SetActive(false);
        UpdateText();
    }
    //get filled stations, update & check if game finished
    private void UpdateCount()
    {
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
}
