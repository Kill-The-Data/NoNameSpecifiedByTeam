using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCargoVisual : MonoBehaviour
{
    [SerializeField] private GameObject m_targetParent = null;

    public void InstantiateObj(GameObject obj)
    {
        GameObject newObj = Instantiate(obj);
        if (m_targetParent)
        {
            newObj.transform.parent = m_targetParent.transform;
            newObj.transform.localScale = Vector3.one;
            newObj.transform.localPosition = Vector3.zero;

        }


        Component[] components = newObj.GetComponents(typeof(Component));
        Debug.Log("length :" + components.Length);
        foreach (Component currentComponent in components)
        {
            if (currentComponent is Rigidbody || currentComponent is Collider || currentComponent is nextTutorialState
                || currentComponent is TrashCollisionHandler || currentComponent is BreakApartHandler)
                Destroy(currentComponent);
        }

    }
    public void RemoveObj(int amount)
    {
        int i = 0;
        foreach (Transform child in m_targetParent.transform)
        {
            if (amount <= i)
            {
                //delay destroy, so that for loop does not complain about removing transforms
                Destroy(child.gameObject, 0.1f);
            }
            i++;
        }

    }
}
