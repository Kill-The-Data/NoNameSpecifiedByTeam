
using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class EasterEggText : MonoBehaviour
{
    private TMP_Text label;
    
    
    private void Awake()
    {
        label = GetComponent<TMP_Text>();
    }

    
    //listen to EasterEggsReceived 
    private void OnEnable()
    {
        EastereggCounter.OnInstance(instance =>
        {
            instance.OnEasterEggReceived += UpdateLabel;
        });
    }

    //un...listen(?) to EasterEggsReceived 
    private void OnDisable()
    {
        EastereggCounter.OnInstance(instance =>
        {
            instance.OnEasterEggReceived -= UpdateLabel;
        });
    }
    
    //update the label
    private void UpdateLabel(int value)
    {
        label.text = value.ToString();
    }
}