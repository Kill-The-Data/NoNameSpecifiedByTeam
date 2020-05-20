using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{

    [Header("---Setup---")]
    [SerializeField] [Range(100, 500)] private int m_spawnDist = 200;
    [SerializeField] private List<GameObject> m_prefabList;
    [SerializeField] private float m_spawnRate = 15.0f;


    [SerializeField] private PlayerScriptContainer m_pScripts = null;
    private float m_timeLeft = 0;
    private PerformanceMeasure m_Measure = null;
    private bool m_active = false;

    //update toimer if active
    void Update()
    {
        if (m_active)
        {
            m_timeLeft -= Time.deltaTime;
            if (m_timeLeft <= 0)
            {
                SpawnItem();
                m_timeLeft = m_spawnRate;
            }
        }
        if (Input.GetKeyDown("space")) InstantiateRandomItem();
    }

    private void InstantiateRandomItem()
    {
        //chose item
        int index = Random.Range(0, m_prefabList.Count);

        GameObject obj = Instantiate(m_prefabList[index]);
        obj.transform.parent = this.transform;

        //get player movement
        Vector3 playerVel = m_pScripts.GetPlayerController.GetVelocity();

        //gen spawn pos
        Vector2 screenpos = Vector2.up;
        int randRot = Random.Range(0, 360);
        screenpos = RotateVec(screenpos, randRot);
        screenpos *= m_spawnDist;
        screenpos += new Vector2(Screen.width / 2, Screen.height / 2);
        Vector3 wPos = Camera.main.ScreenToWorldPoint(new Vector3(screenpos.x, screenpos.y, 10));
        wPos.z = 10;
        wPos += playerVel;
        obj.transform.position = wPos;
    }

    public void Reset()
    {
        m_timeLeft = m_spawnRate;
        if (!m_Measure)
        {
            m_Measure = GetComponent<PerformanceMeasure>();
        }
        m_active = true;
    }
    //Called to stop item spawning
    public void Deactivate()
    {
        m_active = false;
    }
    public Vector2 RotateVec(Vector2 vec, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector2 newVec = Vector2.zero;
        float newX = vec.x * Mathf.Cos(radians) - vec.y * Mathf.Sin(radians);
        float newY = vec.x * Mathf.Sin(radians) + vec.y * Mathf.Cos(radians);
        newVec = new Vector2(newX, newY);
        return newVec;
    }



    private void SpawnItem()
    {
        PerformanceMeasure.Difficulty diff = m_Measure.GetDifficulty();

        int rand = Random.Range(1, (int)diff);
        if (rand == 1) InstantiateRandomItem();
    }
}
