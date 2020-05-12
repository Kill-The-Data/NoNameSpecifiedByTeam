using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LanguageUI : MonoBehaviour
{
    [SerializeField] private Button m_DutchButton;
    [SerializeField] private Button m_EnglishButton;
    [SerializeField] private Color m_SelectedColor;
    [SerializeField] private Color m_normalColor;
    public enum Language
    {
        ENGLISH,
        DUTCH
    }
    private Language m_playerLanguage = Language.ENGLISH;
    private void Start()
    {
        UpdateView();
    }
    private void UpdateView()
    {
        if (m_playerLanguage == Language.ENGLISH) 
        {
            m_EnglishButton.image.color = m_SelectedColor;
            m_DutchButton.image.color = m_normalColor;

        }
        else if (m_playerLanguage == Language.DUTCH) 
        {
            m_DutchButton.image.color = m_SelectedColor;
            m_EnglishButton.image.color = m_normalColor;
        }
    }
    public void SetLanguageEnglish()
    {
        m_playerLanguage = Language.ENGLISH;
        UpdateView();
    }
    public void SetLanguageDutch()
    {
        m_playerLanguage = Language.DUTCH;
        UpdateView();
    }
}
