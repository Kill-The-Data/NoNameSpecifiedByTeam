using System;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class MailText : MonoBehaviour
{
    private TMP_Text label;
    private int max;
    
    void Start()
    {
        label = GetComponent<TMP_Text>();
        MailCounter.OnInstance(instance =>
        {
            max = instance.MaxMail;
        });
    }

    private void OnEnable() => 
        MailCounter.OnInstance(instance =>
        {
            instance.OnMailReceived += UpdateLabel;
        });

    private void OnDisable() =>
        MailCounter.OnInstance(instance =>
        {
            instance.OnMailReceived -= UpdateLabel;
        });


    private void UpdateLabel(int value)
    {
        if(label)
            label.text = $"{value}/{max}";
    }
}
