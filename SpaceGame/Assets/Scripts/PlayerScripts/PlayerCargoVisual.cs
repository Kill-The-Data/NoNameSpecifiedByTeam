using System;
using UnityEngine;

public class PlayerCargoVisual : MonoBehaviour
{
    [SerializeField] private GameObject m_targetParent = null;
    [SerializeField] private float m_ScaleFactor = 5.0f;
    [SerializeField] private float m_TrashOffset = -1.75f;
    [SerializeField] private float m_MinScale = 0.5f;
    [SerializeField] private float m_MaxScale = 2.25f;

    private void Start()
    {
        EventHandler.Instance.TutorialStart += Reset;
    }

    private int m_amount = 0;
    private void OnEnable()
    {
        Reset();
    }
    private void Update()
    {
        Debug.Log(m_amount);

    }
    public void InstantiateObj(GameObject obj)
    {
        GameObject newObj = Instantiate(obj);
        if (m_targetParent)
        {
            newObj.transform.parent = m_targetParent.transform;
            newObj.transform.localScale = Vector3.one;
            newObj.transform.localPosition = new Vector3(0, m_amount * m_TrashOffset, 0);

        }
        m_amount++;

        //since TrashCollisionHandler depends on TrashMovementController
        //it needs to be removed before everything else
        Destroy(newObj.GetComponent(typeof(TrashCollisionHandler)));

        Component[] components = newObj.GetComponents<Component>();

        foreach (Component currentComponent in components)
        {
            if (currentComponent is Rigidbody
                || currentComponent is Collider
                || currentComponent is nextTutorialState
                || currentComponent is BreakApartHandler
                || currentComponent is TrashMovementController
                || currentComponent is DebrisInertia
            )
            {
                Destroy(currentComponent);
            }
        }
        //get mesh bounds & scale object based on size
        Vector3 meshbounds = newObj.GetComponent<MeshFilter>().mesh.bounds.size;
        float scale = Mathf.Clamp(newObj.transform.localScale.x * (m_ScaleFactor / meshbounds.magnitude), m_MinScale, m_MaxScale);
        newObj.transform.localScale = new Vector3(scale, scale, scale);


    }

    public void RemoveObj(int amount)
    {
        for (int i = 0; i < m_targetParent.transform.childCount; ++i)
        {
            if (amount <= i)
            {
                Destroy(m_targetParent.transform.GetChild(i).gameObject);
                m_amount--;
                amount--;
            }
        }
        if (m_amount < 0) m_amount = 0;
    }

    public void Reset()
    {
        for (int i = 0; i < m_targetParent.transform.childCount; ++i)
        {
            Destroy(m_targetParent.transform.GetChild(i).gameObject);
        }
        m_amount = 0;

    }

}