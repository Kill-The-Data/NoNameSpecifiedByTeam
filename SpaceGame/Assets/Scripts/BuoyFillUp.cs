using UnityEngine;

public class BuoyFillUp : MonoBehaviour
{
    [Header(" --- Boy values setup ---")]
    [SerializeField] private int m_MaxFillUp;
    public int GetMaxFillUp() => m_MaxFillUp;

    [Header(" --- UI setup ---")]
    [Range(0, 1.5f)]
    [SerializeField] private float m_tweenSpeed = 1.0f;
    [Tooltip("The slider should never reach 0, only close to 0 so that it does not disappear")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float m_MinSliderValue = 0.1f;

    //[SerializeField] private GameObject m_TargetRadialFillUp = null;
    [SerializeField] private SpriteRenderer m_TargetRenderer = null;

    private int m_currentFillUp;
    private LerpSlider m_Lerper;

    void Awake()
    {
        Init();
        WebConfigHandler.OnFinishDownload(o =>
        {
            o.ExtractInt("buoy_cargo", value => m_MaxFillUp = value);
        });
        
        
    }
    public void Init()
    {
        if (!m_Lerper)
        {
            m_Lerper = m_TargetRenderer.gameObject.AddComponent<LerpSlider>();
        }
        Material mat = m_TargetRenderer.material;
        m_Lerper.Init(mat, m_tweenSpeed, m_MinSliderValue);
        m_currentFillUp = 0;
        m_Lerper.UpdateSlider(0.01f);
    }


    public int DropOff(int dropOffAmount)
    {
        //just return drop off if full
        if (Full())
        {
            return dropOffAmount;
        }

        int remainingCargo = 0;

        // add cargo
        m_currentFillUp += dropOffAmount;

        //set fill up to max if it exceeds max
        if (m_currentFillUp >= m_MaxFillUp)
        {

            remainingCargo = m_currentFillUp - m_MaxFillUp;
            m_currentFillUp = m_MaxFillUp;
            FilledUp();

        }
        //return leftover player cargo if not all cargo can be dropped off
        float fillUp = (float)m_currentFillUp / (float)m_MaxFillUp;
        m_Lerper.UpdateSlider(fillUp + 0.05f);
        return remainingCargo;

    }

    private void FilledUp()
    {
        EventSingleton.Instance?.EventHandler?.NewStationFilled();
    }
    public bool Full()
    {
        return m_currentFillUp >= m_MaxFillUp;
    }
}
