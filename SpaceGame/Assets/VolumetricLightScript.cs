using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumetricLightScript : MonoBehaviour
{
    public Transform playerTransform;
    private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer =GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        renderer.material.SetVector("POS",Camera.main.WorldToScreenPoint(playerTransform.position));
     //   renderer.material.SetVector("ROT", playerTransform);

    }
}
