using UnityEngine;
/// <summary>
/// murders particle after it was played
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class MurderParticle : MonoBehaviour
{
    private void Awake()
    {
        ParticleSystem p = GetComponent<ParticleSystem>();
        float lifeTime = p.duration + p.startLifetime;
        Destroy(gameObject, lifeTime);
    }
}
