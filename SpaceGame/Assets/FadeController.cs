using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Renderer))]
public class FadeController : MonoBehaviour
{
    private Renderer m_R = null;
    [SerializeField] private float m_speed = 1.0f;
    [SerializeField] private LeanTweenType m_TweenType = LeanTweenType.linear;
    [SerializeField] private float m_maxFade = 1.2f;

    private void Awake()
    {
        m_R = GetComponent<Renderer>();
        m_R.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown("space")) Fade();
    }
    public void Fade()
    {
        m_R.enabled=true;
        Debug.Log("Fading");
        LeanTween.value(gameObject, 0, m_maxFade, m_speed).setEase(m_TweenType).setOnUpdate((float val) =>
        {
            m_R.material.SetFloat("FADE", val);
            if (val >= m_maxFade - 0.01f) FadeOut();
        });
    }
    private void FadeOut()
    {
        LeanTween.value(gameObject, m_maxFade, 0, m_speed).setEase(m_TweenType).setOnUpdate((float val) =>
          {
              m_R.material.SetFloat("FADE", val);
              if (val <= 0) m_R.enabled = false;

          });
    }
}
