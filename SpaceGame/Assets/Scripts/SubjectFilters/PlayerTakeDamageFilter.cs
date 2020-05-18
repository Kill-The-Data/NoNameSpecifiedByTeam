using System;
using UnityEngine;

namespace SubjectFilters
{
    public class PlayerTakeDamageFilter : ISubjectFilter
    {
        public void GetUpdate(ISubject subject)
        {
            if (subject is TrashCollisionHandler handler)
            {
                if (handler.State == TrashCollisionHandler.NotifyEvent.OnPlayerTakeDamage)
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