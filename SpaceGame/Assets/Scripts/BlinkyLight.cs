using UnityEngine;

[RequireComponent(typeof(Light))]
public class BlinkyLight : MonoBehaviour
{

  [Tooltip("The Inventory of the Buoy")]
  [LabelOverride("Buoy Inventory")]
  [SerializeField] private BuoyFillUp m_fillUp;
  
  private bool m_isBlinking = false;
  private Light m_blinkyLight;

  public void Awake()
  {
    m_blinkyLight = GetComponent<Light>();
  }
  
  public void Blink()
  {
    m_isBlinking = true;
  }

  private float m_time;
  
  private void Update()
  {

    if (m_fillUp.Full())
    {
      Blink();
    }
    
    m_time += Time.deltaTime;
    
    if (m_isBlinking)
    {
      if (m_time < 0.5F)
      {
        m_blinkyLight.intensity = 0;
      }
      if (m_time > 0.5F)
      {
        m_blinkyLight.intensity = 3;
      }

      if (m_time > 1.0F)
      {
        m_time = 0;
      }
    }
  }
}
