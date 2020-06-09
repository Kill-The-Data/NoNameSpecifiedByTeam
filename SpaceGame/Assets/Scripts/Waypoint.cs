using UnityEngine;
using Random = UnityEngine.Random;

public class Waypoint : MonoBehaviour
{
   public Waypoint PreviousWaypoint;
   public Waypoint NextWaypoint;

   [Range(0f, 5f)] 
   public float Width;
   

   public Vector3 GetPosition()
   {
      var tf = transform;
      var right = tf.right;
      var position = tf.position;
      
      Vector3 minBound = position + right * Width / 2;
      Vector3 maxBound = position - right * Width / 2;

      return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
   }
}
