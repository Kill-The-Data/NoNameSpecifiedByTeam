using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    [SerializeField] private float m_shakeDuration = 0.0f;
    [SerializeField] private float m_shakeStrength = 0.0f;
    [SerializeField] private float m_shakeDecreaseFactor = 0.0f;

    private float m_currentShakeDuration = 0.0f;
    private Transform m_CamTransform = null;
    private Vector3 m_originalPos = Vector3.zero;
    private Vector3 m_translatedPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        m_CamTransform = GetComponent<Transform>();
        m_originalPos = m_CamTransform.position;
    }
    public Vector3 GetTranslatedPos()
    {
        return m_translatedPos;
    }
    public void TriggerShake()
    {
        m_originalPos = m_CamTransform.position;
        m_currentShakeDuration = m_shakeDuration;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            TriggerShake();
        }
    }
    public Vector3 UpdateShake()
    {
        if (m_currentShakeDuration > 0)
        {
            m_originalPos = m_CamTransform.position;
            float c = m_currentShakeDuration / m_shakeDuration;
            Vector3 Offset = Random.insideUnitSphere * (m_shakeStrength * c);
            Offset.z = m_originalPos.z;

            m_currentShakeDuration -= Time.deltaTime * m_shakeDecreaseFactor;

            return Offset;
        }

        m_currentShakeDuration = 0;
        return Vector3.zero;
    }
}
