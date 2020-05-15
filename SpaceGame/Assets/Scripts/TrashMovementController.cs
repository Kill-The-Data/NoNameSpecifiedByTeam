using UnityEngine;

public class TrashMovementController : MonoBehaviour
{

    public float Drag = 0.995f;
    public Vector3 Speed = Vector3.zero;
    
    void Update()
    {
        transform.position += Speed * Time.deltaTime;
        Speed *= Drag;
    }
}
