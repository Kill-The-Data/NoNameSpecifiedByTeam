using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScriptContainer : MonoBehaviour
{
    [SerializeField] private GameObject m_parentObject;

    private PlayerHealth m_Health = null;
    public PlayerHealth GetPlayerHealth => m_Health;

    [SerializeField] private TimerView _timerView = null;
    public Timer GetTimer => _timerView.timer;

    private ShieldState _forceShield = null;
    public ShieldState GetShieldState => _forceShield;
    private void Start()
    {
        m_Health = m_parentObject.GetComponent<PlayerHealth>();
        _forceShield = m_parentObject.GetComponent<ShieldState>();
    }
}
