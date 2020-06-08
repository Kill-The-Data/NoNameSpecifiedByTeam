using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterNameView : AbstractView
{
    [SerializeField] private Button m_button;
    [SerializeField] private TMP_Text m_text;
    public string GetText() => m_text.text;

    public void DisableButton() => m_button.enabled = false;
    public void EnableButton() => m_button.enabled = true;

}
