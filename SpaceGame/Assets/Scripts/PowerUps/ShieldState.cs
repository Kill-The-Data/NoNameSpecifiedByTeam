using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldState : MonoBehaviour
{
    [SerializeField] private GameObject ForceShield;
    [SerializeField] private float fadeSpeed = 0.5f;
    
    public bool IsActive
    {
        get;
        private set;
    } = false;

    public void Init()
    {
        FadeOut();
    }
    void Update()
    {
        if (Input.GetKey("space")) FadeOut();
    }
    public void ActivateShield()
    {
        IsActive = true;

        ForceShield.SetActive(true);
        Material mat = ForceShield.GetComponent<Renderer>().material;
        ForceShield.GetComponent<Renderer>().material.SetFloat("Alpha", 1);
    }
    public  void DeactivateShield() { FadeOut();}
    private void FadeOut()
    {
        //fade out
        LeanTween.value(ForceShield, 1, 0, fadeSpeed).setOnUpdate((float val) =>
        {
            ForceShield.GetComponent<Renderer>().material.SetFloat("Alpha", val);
            ////set unactive once alpha reaches 0
            if (val <= 0)
            {
                ForceShield.SetActive(false);
                IsActive = false;
            }
        });
    }

}
