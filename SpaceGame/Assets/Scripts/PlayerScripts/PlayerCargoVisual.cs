using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerCargoVisual : MonoBehaviour
{
    [SerializeField] private GameObject m_targetParent = null;
    [SerializeField] private float m_ScaleFactor = 5.0f;
    [SerializeField] private float m_TrashOffset = -1.75f;
    [SerializeField] private float m_MinScale = 0.5f;
    [SerializeField] private float m_MaxScale = 2.25f;
    [SerializeField] private int m_MaxCount = 3;
    [SerializeField] private Renderer m_targetRenderer;
    [SerializeField] private Color m_NormalColor = Color.blue;
    [SerializeField] private Color m_FullColor = Color.red;
    private void Start()
    {
        EventHandler.Instance.TutorialStart += Reset;
    }

    private int m_amount = 0;
    private void OnEnable()
    {
        Reset();
    }
    public void InstantiateObj(GameObject obj)
    {
        GameObject newObj = Instantiate(obj);
        //Destroy children off copy -> e.g. particles
        foreach (Transform child in newObj.transform) Destroy(child.gameObject);

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
                || currentComponent is DebrisCollisionSound
            )
            {
                Destroy(currentComponent);
            }
        }
        //get mesh bounds & scale object based on size
        Vector3 meshbounds = newObj.GetComponent<MeshFilter>().mesh.bounds.size;
        float scale = Mathf.Clamp(newObj.transform.localScale.x * (m_ScaleFactor / meshbounds.magnitude), m_MinScale, m_MaxScale);
        newObj.transform.localScale = new Vector3(scale, scale, scale);

        UpdateNet();
    }
    //sets net color
    private void UpdateNet()
    {
        if (m_targetRenderer == null) return;
        Color updateColor;
        //decide if net is full or not
        if (m_amount == m_MaxCount) updateColor = m_FullColor;
        else updateColor = m_NormalColor;
        //update color
        m_targetRenderer.material.SetColor("MAIN_COLOR", updateColor);
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
        UpdateNet();
    }

    public void Reset()
    {
        for (int i = 0; i < m_targetParent.transform.childCount; ++i)
        {
            Destroy(m_targetParent.transform.GetChild(i).gameObject);
        }
        m_amount = 0;
        UpdateNet();
    }

}
