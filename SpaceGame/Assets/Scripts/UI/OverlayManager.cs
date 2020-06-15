using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class OverlayManager : MonoBehaviour
{
    [SerializeField] private float m_duration = 0.5f;
    [SerializeField] private LeanTweenType m_tweenType = LeanTweenType.linear;
    private Color m_ImageColor; 
    private Image m_targetImage = null;

    //grab image
    private void Start()
    {
        m_targetImage = GetComponent<Image>();
        m_ImageColor = m_targetImage.color;
    }

    public void ActivateOverlay()
    {
        //get image if it is null
        if (m_targetImage == null)
        {
            m_targetImage = GetComponent<Image>();
            //do nothing if image could not be found
            if(m_targetImage==null) return;
            //store image color
            m_ImageColor = m_targetImage.color;
        }

        //reset color && activate overlay
        gameObject.SetActive(true);
        m_targetImage.color = m_ImageColor;

        //lerp alpha from 1 to 0, disables gameobject once alpha is <=0
        //uses defined duration & tweenType
        LeanTween.value(this.gameObject, 1, 0, m_duration).setEase(m_tweenType).setOnUpdate((float val) =>
        {
            Color c = m_targetImage.color;
            c.a = val;
            m_targetImage.color = c;
            if (val <= 0) gameObject.SetActive(false);
        });
    }
}
