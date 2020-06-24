using System;
using UnityEngine;

//look at 
public class EastereggCounter : MonoBehaviour
{
   private static EastereggCounter m_instance;
   private static Action<EastereggCounter> m_instanceCreatedActions;

   public event Action<int> OnEasterEggReceived;

   private int m_count = 0;

   public int Count
   {
      get => m_count;
      private set
      {
         m_count = value;
         OnEasterEggReceived?.Invoke(value);
      }
   }

   private void Awake()
   {
      m_instance = this;
      m_instanceCreatedActions?.Invoke(this);
      m_instanceCreatedActions = null;
   }

   public static void OnInstance(Action<EastereggCounter> action)
   {
      if (m_instance)
         action(m_instance);
      else
      {
         m_instanceCreatedActions += action;
      }
   }

   public void AddEasterEgg() => Count++;

}
