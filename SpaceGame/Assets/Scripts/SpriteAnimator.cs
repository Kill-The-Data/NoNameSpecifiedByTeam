using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private bool m_loop;
    [SerializeField] private float m_frameSeconds = 1;
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private Sprite[] sprites;
    private int m_frame = 0;
    private float m_deltaTime = 0;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        m_deltaTime += Time.deltaTime;

      
        while (m_deltaTime >= m_frameSeconds)
        {
            m_deltaTime -= m_frameSeconds;
            m_frame++;
            if (m_loop)
                m_frame %= sprites.Length;
            //Max limit
            else if (m_frame >= sprites.Length)
                m_frame = sprites.Length - 1;
        }
        //Animate sprite with selected m_frame
        m_SpriteRenderer.sprite = sprites[m_frame];
    }
}
