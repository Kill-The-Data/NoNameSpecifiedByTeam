using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractView : MonoBehaviour
{
    public virtual void EnableView()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void DisableView()
    {
        this.gameObject.SetActive(false);
    }
}
