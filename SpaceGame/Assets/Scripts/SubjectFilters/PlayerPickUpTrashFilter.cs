using System;
using UnityEngine;

namespace SubjectFilters
{
    public class PlayerPickUpTrashFilter : ISubjectFilter
    {
        public void GetUpdate(ISubject subject)
        {
            if (subject is TrashCollisionHandler handler)
            {
                if (handler.State == TrashCollisionHandler.NotifyEvent.OnPlayerPickupTrash)
                    Notify();
            }
        }

        private Action<ISubject> m_observers;
        
        public void Notify()
        {
            m_observers(this);
        }

        public void Attach(IObserver observer)
        {
            m_observers += observer.GetUpdate;
        }
    }
}