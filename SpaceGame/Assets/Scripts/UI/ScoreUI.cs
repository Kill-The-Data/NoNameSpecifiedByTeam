using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour, IObserver
{
    [Header("Setup")]
    [SerializeField] private Vector2 m_SpawnPos = Vector2.zero;
    [Tooltip("the prefab for the text that pops up after adding score")]
    [SerializeField] private GameObject m_textPrefab = null;
    [Tooltip("the actual score being displayed ingame")]
    [SerializeField] private TMP_Text m_scoreTargetText = null;

    [Header("tween values")]
    [SerializeField] private float m_Yoffset = 10.0f;
    [SerializeField] private float m_tweenSpeed = 1.0f;


    private int m_currentScore = 0;

    public int GetScore() => m_currentScore;
    public void Reset()
    {
        m_currentScore = 0;
        UpdateView();
    }
    void Update()
    {
        #if UNITY_EDITOR
            if (Input.GetKeyDown("space")) AddScore(10);
        #endif
    }
    public void AddScore(int scoreAmount)
    {
        m_currentScore += scoreAmount;

        //instantiate score pop up
        GameObject tempObj = Instantiate(m_textPrefab);
        tempObj.transform.SetParent(this.transform);
        tempObj.transform.localPosition = new Vector3(m_SpawnPos.x, m_SpawnPos.y, 0);
        tempObj.GetComponent<TMP_Text>().SetText($"+{scoreAmount}");

        //move score up
        tempObj.LeanMoveLocalY(m_SpawnPos.y + m_Yoffset, m_tweenSpeed).setEase(LeanTweenType.linear);

        //fade score out
        LeanTween.value(tempObj, 1, 0, m_tweenSpeed).setOnUpdate((float val) =>
        {
            Color c = tempObj.GetComponent<TMP_Text>().color;
            c.a = val;
            tempObj.GetComponent<TMP_Text>().color = c;
            //delete object once alpha reaches 0
            if (val <= 0) Destroy(tempObj.transform.gameObject);
        });

        UpdateView();
    }
    private void UpdateView()
    {
        m_scoreTargetText.SetText($"{m_currentScore} ");
    }

    public void GetUpdate(ISubject subject)
    {
        if (subject is MotherShipCollisionHandler mCollisionHandler)
        {
            AddScore(mCollisionHandler.ScoreGain);
        }
    }
}
