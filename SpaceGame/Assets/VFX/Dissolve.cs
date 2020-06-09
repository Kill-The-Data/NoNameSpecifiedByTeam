using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private bool m_IsDissolving = false;
    [SerializeField]private float m_dissolveSpeed = 0.5f;
    [SerializeField] private float m_startValue = 1.0f;
    private Renderer m_R;
    private float m_currentValue = 0;
    private void Start()
    {
        m_R = GetComponent<Renderer>();
    }

    void Update()
    {
        if (m_IsDissolving)
        {
            m_currentValue -= Time.deltaTime * m_dissolveSpeed;

            m_R.material.SetFloat("TIME", m_currentValue);

            if (m_currentValue <= 0)
                FinishDissolve();
        }
    }
    private void FinishDissolve()
    {
        m_IsDissolving = false;
        gameObject.SetActive(false);
    }

    //activates dissolving
    public void StartDissolve()
    {
        //only do stuff if you found renderer
        if (m_R)
        {
            m_IsDissolving = true;
            m_currentValue = m_startValue;
            m_R.material.SetFloat("TIME", m_currentValue);
            m_R.material.SetInt("DISSOLVE", 1);
        }
    }
}
