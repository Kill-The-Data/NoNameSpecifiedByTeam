
using System;
using UnityEngine;

public class EasterEgg :MonoBehaviour
{
    public event Action OnEasterEggPickedUp;

    private void Start()
    {
        EastereggCounter.OnInstance(instance =>
        {
            OnEasterEggPickedUp += instance.AddEasterEgg;
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player-Collector"))
        {
            OnEasterEggPickedUp?.Invoke();
        }
    }
}
