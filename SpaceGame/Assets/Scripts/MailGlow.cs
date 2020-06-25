using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GlowController))]
public class MailGlow : MonoBehaviour
{
   private void Start()
   {
      var glow = gameObject.GetComponent<GlowController>();
      MailCounter.OnInstance(instance => instance.OnMailReceived += amount => glow.Animate());
   }
}
