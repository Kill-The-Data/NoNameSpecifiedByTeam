﻿using System;
using UnityEngine;
using System.Collections.Generic;
using csDelaunay;
using Tools;
using UnityEditor;
using Random = UnityEngine.Random;

public class VoronoiDebrisGen : MonoBehaviour
{
    public enum ListSelection
    {
        TRASH,
        ASTEROIDS,
        BIG_OBSTACLES,
        BROKEN_SHIP,
        BROKEN_SATELITE,
        EASTER_EGGS
    };


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


    [Tooltip("The radius from which no-mans land begins")]
    public float NoMansLandRadius = 2000f;
    public float EasterEggDistance = 10f;

    //----------- Trash Setup Variables
    [Header(" --- Setup trash Prefabs --- ")]
    public List<GameObject> TrashList;

    [Header(" --- Setup big Asteroids --- ")]

    public List<GameObject> AsteroidsList;

    [Header(" --- Setup big Obstacles --- ")]
    public List<GameObject> ObstaclesList;

    [Header(" --- Easter eggs --- ")]
    public List<GameObject> EasterEggsList;


    [Header(" --- Broken Ship parts --- ")]
    public List<GameObject> BrokenShipList;

    [Header(" --- Broken Satelite parts --- ")]
    public List<GameObject> BrokenSateliteList;

    //----------- Exclusion Zone Setup Variables
    //----------- takes in a list type that correlates to the list of spawnable objects
    [Serializable]
    public class ExclusionZone
    {
        public Transform Target;
        public float Radius;
        public float OuterRadius;
        public int AmountTrash;
        public VoronoiDebrisGen.ListSelection listType;
        public int InitialTrash { get; set; }
        public bool GeneratesTrash
        {
            get => listType == ListSelection.TRASH;
        }
    }

    [Header(" --- Setup Exclusion Zones ---")]

    [Tooltip("The Areas where space-ship debris should be generated with and inner and outer radius")]
    [SerializeField] private List<ExclusionZone> m_zones = new List<ExclusionZone>();


    public List<ExclusionZone> GetExclusionZones() => m_zones;


    [Header(" --- DEDUPLICATION ---")]
    [Tooltip("Whether or not to draw the Gizmos for the Debris Exclusion Zones")]
    [SerializeField] private bool m_showDebrisExclusions = false;
    [Tooltip("How much space each piece of debris gets on a minimal basis"), Range(0, 10)]
    [SerializeField] private float m_exclusionZoneRadiusForNewDebris;
    [Tooltip("How much space each piece of obstacle gets on a minimal basis"), Range(0, 30)]
    [SerializeField] private float m_exclusionZoneRadiusForNewObstacle;

    private List<ExclusionZone> m_debrisZones = null;

    private Action m_finalShow;
    public bool WasPrewarmed => m_finalShow != null;


    //stores data
    private Dictionary<Vector2f, Site> sites;
    private List<Edge> edges;

    private NotifyAddChildren m_childAdder;

    public void Awake()
    {
        foreach (var zone in m_zones)
        {
            if (zone.Target.GetComponentSafe(out BuoyFillUp bfp))
            {
                zone.AmountTrash = bfp.GetMaxFillUp();
            }

            zone.InitialTrash = zone.AmountTrash;
        }
    }

    public void Generate(bool do_prewarm = false)
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

        m_childAdder = GetComponent<NotifyAddChildren>();

      //  GenerateEasterEggs();

        Display(do_prewarm);
    }

    private void GenerateEasterEggs()
    {

        foreach (GameObject currentObj in EasterEggsList)
        {
            Vector2 randomRadial = Random.insideUnitCircle;
            randomRadial = randomRadial.normalized;
            randomRadial *= (NoMansLandRadius - EasterEggDistance);

            Vector3 pos = new Vector3(randomRadial.x + position.x + NoMansLandRadius / 2 + 10, randomRadial.y + NoMansLandRadius / 2 + position.y + 10, ZCoord);
            GameObject obj = m_childAdder.AddChild(Instantiate(currentObj, pos, Quaternion.identity));
            obj.SetActive(false);
            m_finalShow += () => obj.SetActive(true);

        }
    }
    //Creates random points
    private List<Vector2f> CreateRandomPoint()
    {
        List<Vector2f> points = new List<Vector2f>();
        for (int i = 0; i < TrashAmount; i++)
        {
            points.Add(new Vector2f(Random.Range(0, AreaDimensions * 1 / Offset), Random.Range(0, AreaDimensions * 1 / Offset)));
        }

        return points;
    }
    //Displays and creates Trash
    private void Display(bool do_prewarm)
    {

        Vector3 center = new Vector3(position.x + AreaDimensions, position.y + AreaDimensions, 0) * Offset / 2;
        Vector2 origin = new Vector2(position.x - AreaDimensions * Offset / 2, position.y - AreaDimensions * Offset / 2);


        foreach (Vector2f vec in sites.Keys)
        {
            //position
            Vector3 pos = new Vector3(vec.x * Offset + origin.x, vec.y * Offset + origin.y, ZCoord);

            //check if the trash has been spawned in the death zone
            bool inDeathZone = Vector3.Distance(center, pos) > NoMansLandRadius;


            bool silentAdd = false;

            //Gameobject to instantiate
            GameObject spawnPrefab = null;
            //check all exclusion zones
            if (!CheckNearbyDebris(pos, inDeathZone)) continue;
            if (!inDeathZone)
            {
                var (success, prefab, zone) = CheckGenZones(pos);
                if (!success) continue;

                if (zone != null && zone.GeneratesTrash)
                {
                    silentAdd = !zone.GeneratesTrash;
                    if (!MaximumDebrisCount.AddDebris()) continue;
                }
                spawnPrefab = prefab;
            }



            var exclusionRadius = m_exclusionZoneRadiusForNewDebris;

            if (inDeathZone)
            {
                if (Random.Range(0F, 1F) > 0.5F)
                {
                    spawnPrefab = ObstaclesList[Random.Range(0, ObstaclesList.Count)];
                    exclusionRadius = m_exclusionZoneRadiusForNewObstacle;
                    silentAdd = true;
                }
            }

            //random trash orientation
            float randomFloat = UnityEngine.Random.Range(0, 360);
            Quaternion rotation = Quaternion.Euler(0, 0, randomFloat);

            //instantiate trash
            //make sure Gamobject is not null
            if (spawnPrefab != null)
            {
                GameObject trash = m_childAdder.AddChild(Instantiate(spawnPrefab, pos, rotation));

                if (do_prewarm && !inDeathZone)
                {
                    trash.SetActive(false);
                    m_finalShow += () => trash.SetActive(true);
                }
                m_debrisZones.Add(new ExclusionZone { Target = trash.transform, Radius = exclusionRadius });
            }
        }
    }
    public void DeleteLevel()
    {
        //no personal space
        m_debrisZones?.Clear();
        foreach (var zone in m_zones)
        {
            zone.AmountTrash = zone.InitialTrash;
        }

        MaximumDebrisCount.ClearDebris();

        //doing lots of non fun stuff, let me at least make them fun
        void Murder(GameObject go) => Destroy(go);

        //murder all children
        for (int i = 0; i < transform.childCount; ++i)
        {
            Murder(transform.GetChild(i).gameObject);
        }

        m_finalShow = null;
    }

    //checks if a zone is suitable
    //if suitable zone is found, it selects a random object for the zone
    private (bool, GameObject, ExclusionZone) CheckGenZones(Vector3 pos)
    {
        ExclusionZone current = null;

        bool can_gen = false;
        GameObject GameObjectToGen = null;
        foreach (var zone in m_zones)
        {
            Vector3 test = pos;
            test.z = 0;

            Vector3 target = zone.Target.position;
            target.z = 0;

            //get distance between station and 
            float dist = Vector3.Distance(test, target);

            //check if the trash is to close
            if (dist < zone.Radius)
            {
                return (false, null, null);
            }

            //check if the trash is just right
            if (dist > zone.Radius && dist < zone.OuterRadius)
            {
                //make sure this zone can gen any more trash
                can_gen = zone.AmountTrash-- > 0;

                if (!can_gen)
                {
                    return (false, null, null);
                }

                current = zone;

                //get Random gameobject
                var collection = GetCollection(zone);
                //get a random Object from the collection
                int randomIndex = UnityEngine.Random.Range(0, collection.Count);
                GameObjectToGen = collection[randomIndex];
                if (GameObjectToGen == null) Debug.LogError("Debris Spawn Gameobject is null");
            }
        }


        return (true, GameObjectToGen, current);
    }

    //takes in an exclusion zone and returns the associated spawn List, with list type
    private List<GameObject> GetCollection(ExclusionZone zone)
    {
        switch (zone.listType)
        {
            case ListSelection.ASTEROIDS:
                return AsteroidsList;
            case ListSelection.TRASH:
                return TrashList;
            case ListSelection.BIG_OBSTACLES:
                return ObstaclesList;
            case ListSelection.BROKEN_SATELITE:
                return BrokenSateliteList;
            case ListSelection.BROKEN_SHIP:
                return BrokenShipList;
            case ListSelection.EASTER_EGGS:
                return EasterEggsList;
        }
        return null;
    }
    private bool CheckNearbyDebris(Vector3 pos, bool inDeathZone)
    {
        foreach (var zone in m_debrisZones)
        {
            var radius = zone.Radius;
            if (inDeathZone)
            {
                radius *= 0.5f;
            }

            //prepare collision handler
            Vector3 test = pos;
            test.z = 0;

            Vector3 target = zone.Target.position;
            target.z = 0;

            //check if the distance to one of the gen-zones is to small
            if (Vector3.Distance(test, target) < radius)
            {
                return false;
            }
        }
        return true;
    }




#if (UNITY_EDITOR)
    private static void DrawString(string text, Vector3 worldPos, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();

        Color prevColor = GUI.color;
        if (colour.HasValue)
        {
            GUI.color = colour.Value;
        }

        var view = UnityEditor.SceneView.currentDrawingSceneView;
        if (view == null)
        {
            return;
        }

        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height - 20, size.x, size.y), text);

        GUI.color = prevColor;

        UnityEditor.Handles.EndGUI();
    }

    public void OnDrawGizmos()
    {
        Vector3 center = new Vector3(position.x + AreaDimensions, position.y + AreaDimensions, 0) * Offset / 2;

        if (!m_showGizmos) return;

        foreach (var zone in m_zones)
        {
            //draw the exclusion zone
            UnityEditor.Handles.color = Color.green;
            var targetPosition = zone.Target.position;

            UnityEditor.Handles.DrawWireDisc(targetPosition, Vector3.forward, zone.Radius);
            UnityEditor.Handles.Label(targetPosition + Vector3.down * zone.Radius, "Exclusion Zone for " + zone.Target.name);

            if (zone.OuterRadius != 0)
            {
                Vector3 halfwayPoint = targetPosition + Vector3.left * (zone.Radius + zone.OuterRadius) / 2;

                UnityEditor.Handles.DrawLine(targetPosition + Vector3.left * zone.Radius, halfwayPoint + Vector3.right);
                UnityEditor.Handles.DrawLine(halfwayPoint + Vector3.left, targetPosition + Vector3.left * zone.OuterRadius);

                var centeredStyle = new GUIStyle(GUI.skin.label);



                DrawString(zone.AmountTrash.ToString(), halfwayPoint, zone.AmountTrash > 0 ? Color.red : Color.green);

                UnityEditor.Handles.DrawWireDisc(targetPosition, Vector3.forward, zone.OuterRadius);
                UnityEditor.Handles.Label(targetPosition + Vector3.down * zone.OuterRadius, "Gen Zone for " + zone.Target.name);
            }
        }

        if (m_showDebrisExclusions && m_debrisZones != null) foreach (var zone in m_debrisZones)
            {
                if (!zone.Target) continue;
                //draw the exclusion zone

                var radius = zone.Radius;
                if (Vector3.Distance(center, zone.Target.position) >= NoMansLandRadius)
                {
                    radius *= 0.5f;
                }
                UnityEditor.Handles.color = Color.yellow;
                UnityEditor.Handles.DrawWireDisc(zone.Target.position, Vector3.forward, radius);
            }

        UnityEditor.Handles.color = Color.red;
        //draw the bounding box
        UnityEditor.Handles.DrawWireCube(new Vector3(position.x, position.y, 0) + new Vector3(AreaDimensions, AreaDimensions, 0) * Offset / 2
            , new Vector3(AreaDimensions, AreaDimensions, 0) * Offset * 2);

        UnityEditor.Handles.DrawWireDisc(
            new Vector3(position.x, position.y, 0) + new Vector3(AreaDimensions, AreaDimensions, 0) * Offset / 2,
            Vector3.forward,
            NoMansLandRadius);


    }
#endif


    public void Prewarm()
    {
        Generate(do_prewarm: true);
    }

    public void ApplyPrewarm()
    {
        m_finalShow?.Invoke();
    }

}