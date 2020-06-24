using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailSliderController : MonoBehaviour
{
    private int max;
    private LerpSlider slider;
    void Start()
    {
        slider = gameObject.AddComponent<LerpSlider>();
        slider.Init(GetComponent<Slider>());
        MailCounter.OnInstance(instance =>
        {
            max = instance.MaxMail;
            instance.OnMailReceived += ReceivedValue;
        });
    }

    private void ReceivedValue(int v)
    {
        slider.UpdateSlider((float)v / max);
    }
    
    

}
