using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GlowController : MonoBehaviour
{
    private Image m_image;
    
    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
        
        UpdateAlpha(0);
   
    }

    void UpdateAlpha(float v)
    {
        Color imagecolor = m_image.color;

        imagecolor.a = v;
        
        m_image.color = imagecolor; 
    }

    public void Animate()
    {
        LeanTween.value(gameObject, 0, 1, 0.2F)
            .setEase(LeanTweenType.pingPong)
            .setOnUpdate(UpdateAlpha)
            .setOnComplete(() => LeanTween.value(gameObject, 1, 0, 0.3F)
                .setEase(LeanTweenType.pingPong)
                .setOnUpdate(UpdateAlpha));
    }
    

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
            if(Input.GetKeyDown("0")) Animate();
        #endif
    }
}
