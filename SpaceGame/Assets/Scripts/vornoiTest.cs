using UnityEngine;
using System.Collections.Generic;
using csDelaunay;

public class vornoiTest : MonoBehaviour
{
    [Header(" --- Generation Variables --- ")]
    public int TrashAmount = 200;
    public int AreaDimensions = 512;
    public int zCoord = 10;
    public float offset = 1;


    public int LloydFactor = 1;
    public bool UseloydRelaxation = false;
    public Vector2 position = new Vector2(0, 0);
    [Header(" --- Setup trash Prefabs --- ")]
    public List<GameObject> prefabs;
   
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
        if (UseloydRelaxation) voronoi.LloydRelaxation(LloydFactor);

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
        Vector2 origin = new Vector2(position.x - AreaDimensions * offset / 2, position.y - AreaDimensions * offset / 2);
        foreach (Vector2f vec in sites.Keys)
        {
            int randomIndex = UnityEngine.Random.Range(0, prefabs.Count - 1);

            //random trash orientation
            float randomFloat = UnityEngine.Random.Range(0, 360);
            Quaternion rotation = Quaternion.Euler(0, 0, randomFloat);
            //position
            Vector3 pos = new Vector3(vec.x * offset + origin.x, vec.y * offset + origin.y, zCoord);
            //instantiate trash
            GameObject trash = Instantiate(prefabs[randomIndex], pos, rotation);
            trash.transform.parent = this.transform;
        }

    }


}