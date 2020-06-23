using System;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class MailText : MonoBehaviour
{
    private TMP_Text label;
    void Start()
    {
        label = GetComponent<TMP_Text>();
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
        label.text = $"Mail : {value}";
    }
}
