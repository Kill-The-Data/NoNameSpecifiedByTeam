using System;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using csDelaunay;
using Random = UnityEngine.Random;

public class VornoiDebrisGen : MonoBehaviour
{
    //----------- Setup Variables
    [Header(" --- Setup ---")]
    [Tooltip("Whether or not to show the Gizmos for The Voronoi Params")]
    [SerializeField] private bool m_showGizmos = false;

    //----------- Generation Variables
    [Header(" --- Generation Variables --- ")]

    [Tooltip("how much Trash to Generate")]
    public int TrashAmount = 200;

    [Tooltip("how big the area for generation should be, also affected by Offset")]
    public int AreaDimensions = 512;
    
    [Tooltip("on which plane to generate the garbage on")]
    public int ZCoord = 10;
    
    [Tooltip("how big the distance between garbage-sites should be")]
    public float Offset = 1;
    

    [Tooltip("https://www.jasondavies.com/lloyd/")]
    public int LloydFactor = 1;

    [Tooltip("whether or not to use LloydRelaxation")]
    public bool UseLloydRelaxation = false;

    [Tooltip("The center of the Generation Area")]
    public Vector2 position = new Vector2(0, 0);
    
    //----------- Trash Setup Variables
    [Header(" --- Setup trash Prefabs --- ")]
    public List<GameObject> prefabs;

    //----------- Exclusion Zone Setup Variables
    [Serializable]
    public class ExclusionZone
    {
        public Transform Target;
        public float Radius;
    }
    
    [Header(" --- Setup Exclusion Zones ---")] 

    [Tooltip("The Areas where no debris should be generated")]
    [SerializeField] private List<ExclusionZone> m_zones = new List<ExclusionZone>();

    [Header(" --- DEDUPLICATION ---")]
    [Tooltip("Whether or not to draw the Gizmos for the Debris Exclusion Zones")]
    [SerializeField] private bool m_showDebrisExclusions = false;
    [Tooltip("How much space each piece of debris gets on a minimal basis"), Range(0, 10)]
    [SerializeField] private float m_exclusionZoneRadiusForNewDebris;


    private List<ExclusionZone> m_debrisZones = null;
    
    //stores data
    private Dictionary<Vector2f, Site> sites;
    private List<Edge> edges;
    void Start()
    {
        //Allocate space for the debris exclusion zones, we 
        //want to avoid collisions of debris this way
        m_debrisZones = new List<ExclusionZone>(TrashAmount);
        
        
        // Create your sites center off vornoi cells
        //these points are used as spawn positions for the trash
        List<Vector2f> points = CreateRandomPoint();

        // Create the bounds of the voronoi diagram
        Rectf bounds = new Rectf(0, 0, AreaDimensions, AreaDimensions);

        // There is a two ways you can create the voronoi diagram: with or without the lloyd relaxation
        // Vornoi with lloyd relaxation 
        Voronoi voronoi = new Voronoi(points, bounds);
        if (UseLloydRelaxation) voronoi.LloydRelaxation(LloydFactor);

        // Now retreive the edges from it, and the new sites position if you used lloyd relaxtion
        sites = voronoi.SitesIndexedByLocation;
        //edge calculation is not needed
        //edges = voronoi.Edges;

        Display();
    }
    //Creates random points
    private List<Vector2f> CreateRandomPoint()
    {
        List<Vector2f> points = new List<Vector2f>();
        for (int i = 0; i < TrashAmount; i++)
        {
            points.Add(new Vector2f(Random.Range(0, 512), Random.Range(0, 512)));
        }

        return points;
    }
    //Displays and creates Trash
    private void Display()
    {
        Vector2 origin = new Vector2(position.x - AreaDimensions * Offset / 2, position.y - AreaDimensions * Offset / 2);
        foreach (Vector2f vec in sites.Keys)
        {
            //position
            Vector3 pos = new Vector3(vec.x * Offset + origin.x, vec.y * Offset + origin.y, ZCoord);

            bool stopgen = false;
            
            //check all exclusion zones
            foreach (var zone in m_zones)
            {
                
                //prepare collision handler
                Vector3 test = pos;
                test.z = 0;

                Vector3 target = zone.Target.position;
                target.z = 0;

                //check if the distance to one of the gen-zone is to small
                if (Vector3.Distance(test, target) < zone.Radius)
                {
                    //set stopgen and break the loop early
                    stopgen = true;
                    break;
                }
            }
            
            foreach (var zone in m_debrisZones)
            {
                //prepare collision handler
                Vector3 test = pos;
                test.z = 0;

                Vector3 target = zone.Target.position;
                target.z = 0;

                //check if the distance to one of the gen-zone is to small
                if (Vector3.Distance(test, target) < zone.Radius)
                {
                    //set stopgen and break the loop early
                    stopgen = true;
                    break;
                }
            }
            if(stopgen) continue;

            int randomIndex = UnityEngine.Random.Range(0, prefabs.Count - 1);

            //random trash orientation
            float randomFloat = UnityEngine.Random.Range(0, 360);
            Quaternion rotation = Quaternion.Euler(0, 0, randomFloat);
            
            //instantiate trash
            GameObject trash = Instantiate(prefabs[randomIndex], pos, rotation);
            trash.transform.parent = this.transform;
            
            m_debrisZones.Add(new ExclusionZone{Target=trash.transform,Radius = m_exclusionZoneRadiusForNewDebris});
        }
    }
    #if (UNITY_EDITOR)
    public void OnDrawGizmos()
    {
        if (!m_showGizmos) return;

        foreach (var zone in m_zones)
        {
            //draw the exclusion zone
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(zone.Target.position,Vector3.forward,zone.Radius);
            UnityEditor.Handles.Label(zone.Target.position+Vector3.down*zone.Radius,"Exclusion Zone for " + zone.Target.name);
        }

        if(m_showDebrisExclusions && m_debrisZones != null) foreach (var zone in m_debrisZones)
        {
            if(!zone.Target) continue;
            //draw the exclusion zone
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(zone.Target.position,Vector3.forward,zone.Radius);
        }
        //draw the bounding box
        UnityEditor.Handles.DrawWireCube(new Vector3(position.x,position.y,0) + new Vector3(AreaDimensions,AreaDimensions,0) * Offset/2
            ,new Vector3(AreaDimensions,AreaDimensions,0) * Offset * 2);

    }
    #endif

}