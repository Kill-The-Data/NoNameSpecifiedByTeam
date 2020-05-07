using System;
using UnityEngine;
using System.Collections.Generic;
using csDelaunay;
using Random = UnityEngine.Random;

public class VornoiDebrisGen : MonoBehaviour
{

    [Header(" --- Setup ---")]
    [Tooltip("Whether or not to show the Gizmos for The Voronoi Params")]
    [SerializeField] private bool m_showGizmos = false;

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

    [Header(" --- Setup trash Prefabs --- ")]
    public List<GameObject> prefabs;


    [Serializable]
    public class ExclusionZone
    {
        public Transform Target;
        public float Radius;
    }

    [Header(" --- Setup Exclusion Zones ---")] 

    [Tooltip("The Areas where no debris should be generated")]
    [SerializeField] private List<ExclusionZone> m_zones = new List<ExclusionZone>();
   

    //stores data
    private Dictionary<Vector2f, Site> sites;
    private List<Edge> edges;
    void Start()
    {
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

            foreach (var zone in m_zones)
            {
                Vector3 test = pos;
                test.z = 0;

                Vector3 target = zone.Target.position;
                target.z = 0;


                if (Vector3.Distance(test, target) < zone.Radius)
                {
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
        }

    }
    #if (UNITY_EDITOR)
    public void OnDrawGizmos()
    {
        if (!m_showGizmos) return;

        foreach (var zone in m_zones)
        {
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(zone.Target.position,Vector3.forward,zone.Radius);
            UnityEditor.Handles.Label(zone.Target.position+Vector3.down*zone.Radius,"Exclusion Zone for " + zone.Target.name);
        }

        UnityEditor.Handles.DrawWireCube(new Vector3(position.x,position.y,0),new Vector3(AreaDimensions,AreaDimensions,0) * Offset);

    }
    #endif

}