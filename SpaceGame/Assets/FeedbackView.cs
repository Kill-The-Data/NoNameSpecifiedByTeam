using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackView : AbstractView
{
    [SerializeField] private Button m_goodFeedbackButton;
    [SerializeField] private Button m_mediumFeedbackButton;
    [SerializeField] private Button m_badFeedbackButton;

    [SerializeField] private Button m_backButton;

    public enum FV_ButtonType
    {
        GOOD_FEEDBACK,
        MEH_FEEDBACK,
        BAD_FEEDBACK,
        BACK,
    }
    

    public void SubscribeButton(FV_ButtonType type, Action action)
    {
        new Func<Button>(() =>
        {
            switch (type)
            {
                case FV_ButtonType.GOOD_FEEDBACK:
                    return m_goodFeedbackButton;
                case FV_ButtonType.MEH_FEEDBACK:
                    return m_mediumFeedbackButton;
                case FV_ButtonType.BAD_FEEDBACK:
                    return m_badFeedbackButton;
                case FV_ButtonType.BACK:
                    return m_backButton;
                default:
                    throw new IndexOutOfRangeException("no such button type");
            }
        })().onClick.AddListener(action.Invoke);
    }
}
