using UnityEngine;
using System.Collections.Generic;

using csDelaunay;

public class vornoiTest : MonoBehaviour
{

    // The number of polygons/sites we want
    public int TrashAmount = 200;
    // This is where we will store the resulting data
    private Dictionary<Vector2f, Site> sites;
    private List<Edge> edges;
    public GameObject prefab;
    public int Size = 512;
    public int zCoord = 10;
    public float offset = 1;
    public int LloydRelaxationFactor = 1;
    public bool UseloydRelaxation = false;
    public Vector2 position = new Vector2(0, 0);
    void Start()
    {
        // Create your sites (lets call that the center of your polygons)
        List<Vector2f> points = CreateRandomPoint();

        // Create the bounds of the voronoi diagram
        // Use Rectf instead of Rect; it's a struct just like Rect and does pretty much the same,
        // but like that it allows you to run the delaunay library outside of unity (which mean also in another tread)
        Rectf bounds = new Rectf(0, 0, Size, Size);

        // There is a two ways you can create the voronoi diagram: with or without the lloyd relaxation
        // Here I used it with 2 iterations of the lloyd relaxation
        //Voronoi voronoi = new Voronoi(points, bounds, 5);

        // But you could also create it without lloyd relaxtion and call that function later if you want
        Voronoi voronoi = new Voronoi(points, bounds);
        if (UseloydRelaxation) voronoi.LloydRelaxation(LloydRelaxationFactor);

        // Now retreive the edges from it, and the new sites position if you used lloyd relaxtion
        sites = voronoi.SitesIndexedByLocation;
        edges = voronoi.Edges;

        Display();
    }

    private List<Vector2f> CreateRandomPoint()
    {
        List<Vector2f> points = new List<Vector2f>();
        for (int i = 0; i < TrashAmount; i++)
        {
            points.Add(new Vector2f(Random.Range(0, 512), Random.Range(0, 512)));
        }

        return points;
    }

    private void Display()
    {


        Vector2 origin = new Vector2(position.x - Size * offset / 2, position.y - Size * offset / 2);
        foreach (KeyValuePair<Vector2f, Site> kv in sites)
        {
            float randomFloat = UnityEngine.Random.Range(0, 360);

            Quaternion rotation = Quaternion.Euler(0, 0, randomFloat);

            Vector3 pos = new Vector3(kv.Key.x * offset + origin.x, kv.Key.y * offset + origin.y, zCoord);

            GameObject trash = Instantiate(prefab, pos, rotation);
            trash.transform.parent = this.transform;
        }

    }


}