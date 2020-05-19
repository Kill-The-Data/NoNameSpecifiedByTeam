using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScriptContainer : MonoBehaviour
{
    [SerializeField] private GameObject m_parentObject;

    private PlayerHealth pHealth = null;
    public PlayerHealth GetPlayerHealth => pHealth;
   private void Start()
   {
       pHealth = m_parentObject.GetComponent<PlayerHealth>();
   }
}
