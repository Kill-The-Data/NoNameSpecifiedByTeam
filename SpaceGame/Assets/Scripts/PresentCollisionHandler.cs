using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentCollisionHandler : AbstractCollider
{
    [SerializeField] private bool m_SkipCutScene = true;

    public GoalReachedHandler animationController = null;
    public GameObject particle = null;
    private Renderer m_Renderer = null;
    protected override void HandlePlayerEnter(Collider other)
    {
        //check if other is connected to the player and get all the required components
        if (other.CompareTag("Player-Collector") || other.CompareTag("Player"))
        {
            Vector3 POI = other.ClosestPoint(transform.position);
            if (particle)
                Instantiate(particle, POI, transform.rotation);

            TriggerCutScene();
            Render(false);
        }
    }
    private void Render(bool doRender)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(doRender);
        }
        if (m_Renderer)
            m_Renderer.enabled = doRender;
    }
    private void OnEnable()
    {
        Reset();
        Render(true);
    }
    private void Reset()
    {
        if (m_Renderer == null) m_Renderer = GetComponent<Renderer>();
    }
    private void TriggerCutScene()
    {
        if (m_SkipCutScene) return;
        animationController?.TriggerAnimation();
    }
}


