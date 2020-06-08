using System;
using UnityEngine;
using UnityEngine.UI;

public class FuelGaugeRotator : MonoBehaviour
{
    private float amount = 1;

    [SerializeField] private Timer timer;


    public void SetPercentage(float percentage)
    {
        amount = percentage;
        if (amount < 0) amount = 0;
        if (amount > 1) amount = 1;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.AngleAxis(-180 * amount, Vector3.forward),Time.deltaTime);
    }
}
