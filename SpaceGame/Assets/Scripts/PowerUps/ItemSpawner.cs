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

    [SerializeField] private VoronoiDebrisGen m_debrisGen;

    private List<VoronoiDebrisGen.ExclusionZone> m_zones;
    private Camera m_camera;
    
    public void Start()
    {
        m_zones = m_debrisGen.GetExclusionZones();
        m_camera = Camera.main;
    }

    //update timer if active
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
        
        //get player movement
        Vector3 playerVel = m_pScripts.GetPlayerController.GetVelocity();
        
        //retry 10 times to spawn an item if spawning fails due to being in an exclusion zone
        for (int i = 10; i --> 0;)
        {
            
            //gen spawn pos
            Vector2 screenpos = Vector2.up;
            
            //generate random rotation
            int randRot = Random.Range(0, 360);
            
            //create screenpos direction
            screenpos = RotateVec(screenpos, randRot);
            
            //extend the direction
            screenpos *= m_spawnDist;
            
            //center the position
            screenpos += new Vector2(Screen.width / 2F, Screen.height / 2F);
            
            //transform to world position
            Vector3 wPos = m_camera.ScreenToWorldPoint(new Vector3(screenpos.x, screenpos.y, 10));
            wPos.z = 10;
            
            //match the player Speed to adjust for flybys
            wPos += playerVel;

            //make sure this position is outside of the exclusion zones we stole from voronoi-debris-gen 
            bool outerLoopBreaker = false;
            foreach (var ezones in m_zones)
            {
                if (Vector3.Distance(ezones.Target.position , wPos) <= ezones.Radius)
                {
                    outerLoopBreaker = true;
                    break;
                }
            }
            if(outerLoopBreaker) continue;
            
            
            //sucesfully created a game object, return as no more powerups need to be created
            GameObject obj = Instantiate(m_prefabList[index], this.transform, true);
            obj.transform.position = wPos;
            return;
        }
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
        float newX = vec.x * Mathf.Cos(radians) - vec.y * Mathf.Sin(radians);
        float newY = vec.x * Mathf.Sin(radians) + vec.y * Mathf.Cos(radians);
        var newVec = new Vector2(newX, newY);
        return newVec;
    }
    
    private void SpawnItem()
    {
        PerformanceMeasure.Difficulty diff = m_Measure.GetDifficulty();

        int rand = Random.Range(1, (int)diff);
        if (rand == 1) InstantiateRandomItem();
    }
}
