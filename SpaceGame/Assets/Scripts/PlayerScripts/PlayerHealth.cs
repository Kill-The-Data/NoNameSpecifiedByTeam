using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [Header(" --- Setup --- ")] 
    [Tooltip("The Maximum health of the player")] 
    [SerializeField] private int m_maxHealth = 100;
    [SerializeField] private TMP_Text m_text = null;
    [SerializeField] private Slider m_slider = null;
    [SerializeField] private ScreenShake m_shake = null;

    [Header(" --- Slider UI setup ---")]
    [Range(0, 1.5f)]
    [SerializeField] private float m_tweenSpeed = 1.0f;
    [Tooltip("The slider should never reach 0, only close to 0 so that it does not disappear")]
    [Range(0.01f,0.2f)]
    [SerializeField] private float m_MinSliderValue = 0.1f;

    [Tooltip("Whether or not the Player can die currently")] 
    [SerializeField] private bool m_canDie = true;


    [Header(" --- Gameplay --- ")] 
    [LabelOverride("Current Health")]
    [Tooltip("The current health of the player (also the starting health)")]
    [SerializeField] private int HealthImpl = 100;
    
    public int Health {
        get => HealthImpl;
        private set
        {
            HealthImpl = value;
            UpdateView();
        }
    }

    private LerpSlider m_lerp = null;

    
    //in the beginning update the text manually to avoid displaying "New Text"
    //and setup the slider ui variables


    public void ResetPlayerHealth()
    {
        Health = m_maxHealth;
        InitSlider();
        UpdateView();
    }

    //Initialize the Slider
    private void InitSlider()
    {
        m_slider.maxValue = m_maxHealth;
        m_slider.minValue = 0;
        m_lerp = m_slider.gameObject.AddComponent<LerpSlider>();
        m_lerp.Init(m_slider, m_tweenSpeed, m_MinSliderValue);
    }
    
    public void TakeDamage(int amount = 10)
    {
       if(amount > 0 && Health > 0) 
           Health = Mathf.Max(Health-amount,0);

        m_shake.TriggerShake();
    }
    
    public void Heal(int amount = 10)
    {
        if (amount > 0 && Health < m_maxHealth)
            Health = Mathf.Max(Health + amount, m_maxHealth);
    }

    public bool IsDead()
    {
        return Health == 0 && m_canDie;
    }

    public void Kill()
    {
        Health = 0;
    }
    
    public bool IsAlive()
    {
        return !IsDead();
    }

    public void UpdateView()
    {
        UpdateSlider();
        UpdateText();
    }
    private void UpdateSlider()
    {
        if (m_lerp != null) m_lerp.UpdateSlider(Health);
    }
    private void UpdateText()
    {
        m_text.SetText(Health != 0 ? $"{Health} / {m_maxHealth}" : "");
    }
}
