using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using EventHandler = SpaceGame.EventHandler;
[RequireComponent(typeof(ShieldState))]
public class PlayerHealth : MonoBehaviour, ISubject
{

    [Header(" --- Setup --- ")] 
    [Tooltip("The health glow effect")]
    [SerializeField] private GlowController m_glow;
    
    [LabelOverride("Use Global Health")]
    [Tooltip("Uses The Variable from the conf server instead")]
    [SerializeField] private bool m_overrideHealthConf = false;

    [Tooltip("The Maximum health of the player")]
    [SerializeField] private int m_maxHealth = 100;

    [Tooltip("The text to display the health in")]
    [SerializeField] private TMP_Text m_text = null;

    [Tooltip("The slider to display the remaining amount of health in")]
    [SerializeField] private Slider m_slider = null;

    [Tooltip("the screenshake animation when the player takes damage")]
    [SerializeField] private ScreenShake m_shake = null;

    [Header(" --- Slider UI setup ---")]
    [Range(0, 1.5f)]
    [SerializeField] private float m_tweenSpeed = 1.0f;
    [Tooltip("The slider should never reach 0, only close to 0 so that it does not disappear")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float m_MinSliderValue = 0.1f;
    [SerializeField] private OverlayManager m_OverlayManager;

    [Tooltip("Whether or not the Player can die currently")]
    [SerializeField] private bool m_canDie = true;


    [Header(" --- Gameplay --- ")]
    [LabelOverride("Current Health")]
    [Tooltip("The current health of the player (also the starting health)")]
    [SerializeField] private int HealthImpl = 100;

    private ShieldState m_shieldState = null;
    public int Health
    {
        get => HealthImpl;
        private set
        {
            HealthImpl = value;
            Notify();
            UpdateView();
        }
    }
    public int GetMaxHealth() => m_maxHealth;
    private List<IObserver> observers = new List<IObserver>();

    //in the beginning update the text manually to avoid displaying "New Text"
    //and setup the slider ui variables
    private void Awake()
    {
        EventHandler.Instance.TutorialStart += () => ResetPlayerHealth(false);
        ResetPlayerHealth(false);
    }

    public void ResetPlayerHealth(bool is_awake = false)
    {
        if (!m_shieldState) m_shieldState = GetComponent<ShieldState>();
        m_shieldState?.Init();
        observers = new List<IObserver>();

        if (m_overrideHealthConf && !is_awake)
        {
            WebConfigHandler.OnFinishDownload(o =>
            {
                o.ExtractInt("health", v =>
                {
                    m_maxHealth = v;
                    Health = m_maxHealth;
                    InitSlider();
                    UpdateView();
                });
            });
        }
        Debug.Log("rest hp");
        
        InitSlider();
        Health = m_maxHealth;
    }


    //Initialize the Slider
    private void InitSlider()
    {
        m_slider.maxValue = m_maxHealth;
        m_slider.minValue = 0;

        LerpSlider lerper = m_slider.gameObject.AddComponent<LerpSlider>();
        lerper.Init(m_slider, m_tweenSpeed, m_MinSliderValue);
        Attach(lerper);
    }

    public void TakeDamage(int amount = 10)
    {
        //only take dmg if no shield active
        if (m_shieldState.IsActive)
        {
            m_shieldState.DeactivateShield();
        }
        else
        {
            Health = Mathf.Max(Health - amount, 0);

            m_OverlayManager?.ActivateOverlay();
        }
        m_shake.TriggerShake();
    }

    public void Heal(int amount = 10)
    {
        if (amount > 0 && Health < m_maxHealth)
        {
            Health = Mathf.Clamp(Health + amount, 0, m_maxHealth);
            m_glow.Animate();
        }
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
        UpdateText();
    }

    private void UpdateText()
    {
        m_text.SetText(Health != 0 ? $"{Health} / {m_maxHealth}" : "");
    }

    public void Notify()
    {
        foreach (IObserver observer in observers)
        {
            observer?.GetUpdate(this);
        }
    }

    public void Attach(IObserver observer)
    {
        observers?.Add(observer);
        Notify();
    }
}
