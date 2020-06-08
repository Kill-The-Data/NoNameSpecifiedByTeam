using Tools;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakApartHandler : MonoBehaviour
{
    [Range(1,5)]
    public int Amount = 1;
    public float Phi = 10;
    public float Chance = 0.1f;

    public const float MIN_SCALE_MAGNITUDE = 0.3F;
    
    
    private Vector3 RotateVectorXYPlane(float angle, Vector3 target)
    {
        Vector3 result = new Vector3
        {
            x = Mathf.Cos(angle) * target.x - Mathf.Sin(angle) * target.y,
            y = Mathf.Sin(angle) * target.x + Mathf.Cos(angle) * target.y
        };
        return result;
    }

    public void Break()
    {
        if (this.transform.localScale.magnitude < MIN_SCALE_MAGNITUDE) return;
        
        if (this.GetComponentSafe<TrashMovementController>(out var controller))
        {
            if(controller.Speed.magnitude > 0){
                for (int i = 0; i < Amount; ++i)
                {
                    if(!MaximumDebrisCount.AddDebris()) continue;
                    var instance = Instantiate(this.gameObject);
                    if (transform.parent.GetComponentSafe(out NotifyAddChildren nac))
                    {
                        nac.AddChild(instance);
                    }

                    float scaleDivisor = 1.0f / Amount;

                    instance.transform.localScale = Vector3.one * scaleDivisor * transform.localScale.magnitude;
                   
                    var n = controller.Speed.normalized;
                    float phiHalf = Phi / 2;
                    float divisor = 1.0f / ((float) Amount - 1);
                    
                    
                    float angle = Mathf.Lerp(-phiHalf, phiHalf, divisor * i);
                    var interp = RotateVectorXYPlane(angle * Mathf.Deg2Rad, n).normalized;

                    if (instance.GetComponentSafe(out TrashMovementController innerController))
                    {
                        innerController.Speed = interp.normalized * controller.Speed.magnitude;
                        if (instance.GetComponentSafe(out TrashCollisionHandler tc_handler))
                        {
                            tc_handler.enabled = false;
                            innerController.StartCoroutine(CoHelper.WaitAndDo(4, () =>
                            {
                                tc_handler.enabled = true;
                            }));
                        }
                    }
                    else
                    {
                        Destroy(instance.GetComponent(typeof(TrashCollisionHandler)));
                    }
                }
            }
            MaximumDebrisCount.RemoveDebris();
            Destroy(this.gameObject);
        }
    }

    public void MaybeBreak()
    {
        float p = Random.Range(0, 1);
        
        if(p <= Chance)
            Break();
    }

#if(UNITY_EDITOR)
    public void OnDrawGizmos()
    {
        var n = Vector3.left;

        if (this.GetComponentSafe(out TrashMovementController controller))
        {
            n = -controller.Speed.normalized;
        }
        float phiHalf = Phi / 2;
        var position = transform.position;
        float divisor = 1.0f / ((float) Amount - 1);

        for (int i = 0; i < Amount; ++i)
        {
            float angle = Mathf.Lerp(-phiHalf, phiHalf, divisor * i);
            var interp = RotateVectorXYPlane(angle * Mathf.Deg2Rad, n).normalized;
            Gizmos.DrawLine(position, position + interp * 2);
        }

        var start = RotateVectorXYPlane(phiHalf * Mathf.Deg2Rad, n);
        Handles.DrawWireArc(position, Vector3.back, start, Phi, 2);
    }
#endif
}