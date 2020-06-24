using System;
using System.Collections.Generic;
using System.Linq;
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
  
  [Range(0,1)]
  [SerializeField] private float m_fxVolume = 1;


  [SerializeField] private Transform m_audioRoot;
  
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
  
  private AudioSource MakeMeAnOutput(string name)
  {
      AudioSource src = new GameObject(name,typeof(AudioSource)).GetComponent<AudioSource>();
      src.transform.parent = m_audioRoot;
      src.transform.localPosition = Vector3.zero;
      src.playOnAwake = false;
      return src;
  }

  private AudioSource GetMeAnOutput(string name)
  {
      var bearer = m_audioRoot.Find(name);
      if (bearer == null) return null;
      return bearer.GetComponent<AudioSource>();
  }

  public AudioSource FetchMeAnOutput(string name)
  {
      var src = GetMeAnOutput(name);
      if (src == null) return MakeMeAnOutput(name);
      else return src;
  }
  
  
  public AudioClip GetSound(string name)
  {
      return  m_entries.FirstOrDefault(x => x.Name == name)?.Sound;
  }

  public float GetFxVolume()
  {
      return m_fxVolume;
  }
}
