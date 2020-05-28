using Tools;
using UnityEngine;

public class DebrisPickupLinker : AUnityObserver
{
    [SerializeField] private AUnityObserver m_target;
    [SerializeField] private GameObject m_debrisField;
    private NotifyAddChildren m_nac;

    private void Awake()
    {
        if (m_debrisField.GetComponentSafe(out NotifyAddChildren nac))
        {
            nac.Attach(this);
            m_nac = nac;
        }
    }

    protected override void AGetUpdate(ISubject subject)
    {
        if(m_nac.LastAdded.GetComponentSafe(out TrashCollisionHandler tch))
        {
            tch.playerPickUpTrashFilter.Attach(m_target);
        }
    }
}