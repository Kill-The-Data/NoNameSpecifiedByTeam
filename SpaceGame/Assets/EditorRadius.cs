using UnityEngine;

[ExecuteInEditMode]
public class EditorRadius : MonoBehaviour
{
    [Tooltip("Updates in Edtior Time")]
    [SerializeField] private float m_Radius = 1.0f;
    void Start()
    {
        Reset();
    }
    void OnValidate()
    {
        Reset();
    }

    void Reset()
    {
        transform.localScale = new Vector3(m_Radius, m_Radius, m_Radius);

    }
}
