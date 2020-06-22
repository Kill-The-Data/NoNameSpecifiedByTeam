using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class UpdateBuoyText : MonoBehaviour
{
    [SerializeField] BuoyFillUp m_buoyFill = null;
    private TMP_Text m_tmpText = null;
    private int m_MaxFill = 0;
    private void OnEnable()
    {
        Reset();
    }
    private void Reset()
    {
        //get max fill
        if (m_buoyFill != null) m_MaxFill = m_buoyFill.GetMaxFillUp(); 
        //get text component
        if (m_tmpText == null) m_tmpText = GetComponent<TMP_Text>();
        //update text
        string newText = "0/" + m_MaxFill.ToString();
        m_tmpText.SetText(newText);
    }

    public void UpdateText(float fill)
    {
        Debug.Log("updating text" + fill);
        int count = Mathf.RoundToInt(fill * m_MaxFill);
        string newText = count.ToString() + "/" + m_MaxFill.ToString();

        m_tmpText.SetText(newText);
    }
}
