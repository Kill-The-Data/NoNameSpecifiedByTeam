using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class WebglCompatVideoPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vp = GetComponent<VideoPlayer>();

        vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "space_trailer_transcoded_web.mp4");
        vp.isLooping = true;
        vp.Play();
    }
}
