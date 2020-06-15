using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomOsk : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_text;
    [Range(1,2)]
    [SerializeField] private float m_scale;

    private Button[] m_buttons;
    
    void InsertKeyAction(string character)
    {
        m_text.text += character;
    }

    void DelKeyAction()
    {
        if (m_text.text.Length > 0)
        {
            m_text.text = m_text.text.Substring(0, m_text.text.Length - 1);
        }
    }
    
    void Start()
    {
        //if not unity webgl, make the keyboard a bit bigger
        #if !UNITY_WEBGL
            transform.localScale = new Vector3(m_scale,m_scale,1);
        #endif
        
        m_buttons = GetComponentsInChildren<Button>();
        foreach (var button in m_buttons)
        {
            if (button.name != "Del" && button.name != "Space")
            {
                button.onClick.AddListener(() => InsertKeyAction(button.name));
            }
            else if (button.name == "Space")
            {
                button.onClick.AddListener(()=>InsertKeyAction(" "));
            }
            else
            {
                button.onClick.AddListener(DelKeyAction);
            }
        }
    }
}
