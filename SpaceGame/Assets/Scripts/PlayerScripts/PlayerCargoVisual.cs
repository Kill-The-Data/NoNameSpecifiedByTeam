using System;
using UnityEngine;

public class PlayerCargoVisual : MonoBehaviour
{
    [SerializeField] private GameObject m_targetParent = null;

    private void Start()
    {
        EventHandler.Instance.TutorialStart += Reset;
    }

    public void InstantiateObj(GameObject obj)
    {
        GameObject newObj = Instantiate(obj);
        if (m_targetParent)
        {
            newObj.transform.parent = m_targetParent.transform;
            newObj.transform.localScale = Vector3.one;
            newObj.transform.localPosition = Vector3.zero;
        }
        //since TrashCollisionHandler depends on TrashMovementController
        //it needs to be removed before everything else
        //
        Destroy(newObj.GetComponent(typeof(TrashCollisionHandler)));
        
        Component[] components = newObj.GetComponents<Component>();
        
        foreach (Component currentComponent in components)
        {
            if (currentComponent is Rigidbody
                || currentComponent is Collider
                || currentComponent is nextTutorialState
                || currentComponent is BreakApartHandler
                || currentComponent is TrashMovementController
            )
            {
                Destroy(currentComponent);
            }
        }
    }

    public void RemoveObj(int amount)
    {
        for (int i = 0; i < m_targetParent.transform.childCount; ++i)
            if (amount <= i)
                Destroy(m_targetParent.transform.GetChild(i).gameObject);
    }

    public void Reset()
    {
        for (int i = 0; i < m_targetParent.transform.childCount; ++i)
        {
            Destroy(m_targetParent.transform.GetChild(i).gameObject);
        }
    }
    
}