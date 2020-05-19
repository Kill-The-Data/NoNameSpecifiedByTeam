using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class ItemSpawner : MonoBehaviour
{

    [SerializeField] [Range(100, 500)] private int m_spawnDist = 200;
    [SerializeField] private List<GameObject> m_prefabList;
    void Start()
    {
        Reset();
    }
    void Update()
    {
        if (Input.GetKeyDown("space")) SpawnItem();
    }

    private void SpawnItem()
    {
        int index = Random.Range(0, m_prefabList.Count - 1);

        GameObject obj = Instantiate(m_prefabList[index]);
        obj.transform.parent = this.transform;


        Vector2 screenpos = Vector2.up;
        int randRot = Random.Range(0, 360);
        screenpos = RotateVec(screenpos, randRot);
        screenpos *= m_spawnDist;
        screenpos += new Vector2(Screen.width / 2, Screen.height / 2);
        Debug.Log(screenpos);
        Vector3 wPos = Camera.main.ScreenToWorldPoint(new Vector3(screenpos.x, screenpos.y, 10));
        wPos.z = 10;
        obj.transform.position = wPos;
        Debug.Log(wPos);

    }

    private void Reset() { }
    public Vector2 RotateVec(Vector2 vec, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector2 newVec = Vector2.zero;
        float newX = vec.x * Mathf.Cos(radians) - vec.y * Mathf.Sin(radians);
        float newY = vec.x * Mathf.Sin(radians) + vec.y * Mathf.Cos(radians);
        newVec = new Vector2(newX, newY);
        return newVec;
    }
}
