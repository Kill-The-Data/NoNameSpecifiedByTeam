using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class Dissolve : MonoBehaviour
{
    private enum State
    {
        DISSOLVE,
        REVERSE_DISSOLVE,
        DISSABLED
    }
    [SerializeField] private float m_dissolveSpeed = 0.5f;
    private Renderer m_R;
    private float m_currentValue = 0;
    [SerializeField] private MeshRenderer m_Renderer=null;
    
    private State m_currentState = State.DISSABLED;

    void Update()
    {
        //return if dissabled
        if (m_currentState == State.DISSABLED) return;
        //decrease value if dissolve
        if (m_currentState == State.DISSOLVE)
        {
            m_currentValue -= Time.deltaTime * m_dissolveSpeed;
            //dissable if 0 is reached
            if (m_currentValue <= 0)
            {
                FinishDissolve();
                gameObject.SetActive(false);
            }
        }
        //increase value if reverse
        else if (m_currentState == State.REVERSE_DISSOLVE)
        {
            Debug.Log(m_currentValue);
            m_currentValue += Time.deltaTime * m_dissolveSpeed;
            //disable if max is reached
            if (m_currentValue >= 1)
            {
                FinishDissolve();
            }
        }
        UpdateRendere();
    }

    private void FinishDissolve()
    {
        m_currentState = State.DISSABLED;
        SetDissolveActive();
    }

    //activates reverse dissolve -> is used for enabling shader
    public void StartReverseDissolve()
    {
        m_currentState = State.REVERSE_DISSOLVE;
        m_currentValue = 0;
        SetDissolveActive();
        UpdateRendere();
    }
    public void StartDissolve()
    {
        m_currentState = State.DISSOLVE;
        m_currentValue = 1;
        SetDissolveActive();
        UpdateRendere();
    }
    //shader update functions
    private void SetDissolveActive()
    {
        foreach (Material m in m_Renderer.materials)
            m.SetInt("DISSOLVE", 1);
    }
    private void SetDissolveNonActive()
    {
        foreach (Material m in m_Renderer.materials)
            m.SetInt("DISSOLVE", 0);
      }
    private void UpdateRendere()
    {
        foreach (Material m in m_Renderer.materials)
            m.SetFloat("TIME", m_currentValue);
    }

  
}
