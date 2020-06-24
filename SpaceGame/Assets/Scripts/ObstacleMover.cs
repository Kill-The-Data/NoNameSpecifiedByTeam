using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
   public Vector3 Speed;

   [Range(0,1)]
   public float Drag = 0.995F;

   private void Update()
   {
      transform.position += Speed * Time.deltaTime;
      Speed *= Drag;
   }
}
