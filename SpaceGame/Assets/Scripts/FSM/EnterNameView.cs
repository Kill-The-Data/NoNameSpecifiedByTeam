using TMPro;
using UnityEngine;

public class EnterNameView : AbstractView
{
    [SerializeField] private TMP_Text m_text;
    public string GetText() => m_text.text;
}
