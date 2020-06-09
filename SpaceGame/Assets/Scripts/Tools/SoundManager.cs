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

  private static Action<SoundManager> OnAwake = delegate {  };
  private static bool awoken = false;
  
  public static SoundManager Instance
  {
      get;
      private set;
  }
  
  private void Awake()
  {
      Instance = this;
      OnAwake(this);
      awoken = true;
  }

  [Obsolete("use ExecuteOnAwake with parameter in action")]
  public static void ExecuteOnAwake(Action action)
  {
      if (!awoken)
      {
          OnAwake += x => action();
      }
      else
      {
          action();
      }
  }

  public static void ExecuteOnAwake(Action<SoundManager> action)
  {
      if (!awoken)
      {
          OnAwake += action;
      }
      else
      {
          action(SoundManager.Instance);
      }
  }
  
  
  
  public AudioClip GetSound(string name)
  {
      return  m_entries.FirstOrDefault(x => x.Name == name)?.Sound;
  }
}
