using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerScriptContainer : MonoBehaviour
{
    [SerializeField] private GameObject m_parentObject;
    public GameObject PlayerObject => m_parentObject;

    private PlayerHealth m_Health = null;
    public PlayerHealth GetPlayerHealth => m_Health;

    [SerializeField] private TimerView _timerView = null;
    public Timer GetTimer => _timerView.timer;

    private ShieldState m_forceShield = null;
    public ShieldState GetShieldState => m_forceShield;
    private PlayerController m_pController = null;
    public PlayerController GetPlayerController => m_pController;
    private void Start()
    {
        m_Health = m_parentObject.GetComponent<PlayerHealth>();
        m_forceShield = m_parentObject.GetComponent<ShieldState>();
        m_pController = m_parentObject.GetComponent<PlayerController>();
    }
}
