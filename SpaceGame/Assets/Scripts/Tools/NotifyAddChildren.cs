using System;
using UnityEngine;

namespace Tools
{
    public class NotifyAddChildren : AUnitySubject
    {
        public GameObject LastAdded { get; private set; }
        
        public GameObject AddChild(GameObject go)
        {
            go.transform.parent = this.transform;
            LastAdded = go;
            Notify();

            return go;
        }
        
        private Action<ISubject> m_observers = delegate(ISubject subject) {  };
        protected override void ANotify()
        {
            m_observers(this);
        }

        protected override void AAttach(IObserver observer)
        {
            m_observers += observer.GetUpdate;
        }
    }
}