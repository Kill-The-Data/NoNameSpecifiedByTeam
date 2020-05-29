using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[Serializable]
class SoundEntry
{
    public string Name;
    public AudioClip Sound;
}


public class SoundManager : MonoBehaviour
{
  [SerializeField] private List<SoundEntry> m_entries  = new List<SoundEntry>();

  private static Action OnAwake = delegate {  };
  private static bool awoken = false;
  
  public static SoundManager Instance
  {
      get;
      private set;
  }
  
  private void Awake()
  {
      Instance = this;
      OnAwake();
      awoken = true;
  }

  public static void ExecuteOnAwake(Action action)
  {
      if (!awoken)
      {
          OnAwake += action;
      }
      else
      {
          action();
      }
  }
  
  
  public AudioClip GetSound(string name)
  {
      return  m_entries.FirstOrDefault(x => x.Name == name)?.Sound;
  }
}
