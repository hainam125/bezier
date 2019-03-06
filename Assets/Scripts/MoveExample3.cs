using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MoveExample3 : MonoBehaviour
{
    public const float CurveDistance = 4;
    public const float Scale = 4;

    [SerializeField]
    private Transform intersectionsTransform;
    [SerializeField]
    private Transform roadsTransform;

    private Vehicle[] vehicles;
    private Intersection[] intersections;
    private Dictionary<Intersection, IntersectionInfo> toIntersection;
    
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        CheckInvicity();
        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].UpdateGame(deltaTime);
        }
    }

    private void CheckInvicity()
    {
        for (int i = 0; i < intersections.Length; i++)
        {
            var intersection = intersections[i];
            var intersectionInfo = toIntersection[intersection];
            intersectionInfo.vehicles.Clear();
            intersectionInfo.isOccupied = false;
            for (int v = 0; v < vehicles.Length; v++)
            {
                var vehicle = vehicles[v];
                var to = intersection.Pos - vehicle.Pos;
                var sqrDist = Vector3.SqrMagnitude(to);
                var isNear = sqrDist < Scale * Scale * 1.25f;
                var isAway = Vector3.Dot(to, vehicle.Heading) < 0;
                if (sqrDist <= Scale * Scale * 0.5f)
                {
                    vehicle.isHalt = false;
                    intersectionInfo.isOccupied = true;
                }
                else if (isNear && !isAway)
                {
                    intersectionInfo.vehicles.Add(vehicle);
                }
            }
            
            if (intersectionInfo.isOccupied)
            {
                for (int v = 0; v < intersectionInfo.vehicles.Count; v++)
                {
                    intersectionInfo.vehicles[v].isHalt = true;
                }
            }
            else
            {
                for (int v = 0; v < intersectionInfo.vehicles.Count; v++)
                {
                    intersectionInfo.vehicles[v].isHalt = v != 0;
                }
            }
        }
    }

    private void Init()
    {
        vehicles = FindObjectsOfType<Vehicle>();
        var roads = new Road[roadsTransform.childCount];
        for(int i = 0; i < roads.Length; i++)
        {
            roads[i] = new Road(roadsTransform.GetChild(i));
        }

        toIntersection = new Dictionary<Intersection, IntersectionInfo>();
        intersections = new Intersection[intersectionsTransform.childCount];
        for (int i = 0; i < intersections.Length; i++)
        {
            intersections[i] = new Intersection(intersectionsTransform.GetChild(i));
            toIntersection[intersections[i]] = new IntersectionInfo();
        }

        foreach (var i in intersections)
        {
            i.roads = new Road[4];
            var road0 = roads.Where(x => x.GridY == i.GridY && x.GridX < i.GridX).OrderBy(x => x.GridX).LastOrDefault();
            if (road0 != null && road0.GridX + (road0.Bound.x * 0.5f + Scale * 0.5f) == i.GridX) i.roads[0] = road0;
            
            var road2 = roads.Where(x => x.GridY == i.GridY && x.GridX > i.GridX).OrderBy(x => x.GridX).FirstOrDefault();
            if (road2 != null && road2.GridX - (road2.Bound.x * 0.5f + Scale * 0.5f) == i.GridX) i.roads[2] = road2;

            var road1 = roads.Where(x => x.GridX == i.GridX && x.GridY > i.GridY).OrderBy(x => x.GridY).FirstOrDefault();
            if (road1 != null && road1.GridY - (road1.Bound.y * 0.5f + Scale * 0.5f) == i.GridY) i.roads[1] = road1;

            var road3 = roads.Where(x => x.GridX == i.GridX && x.GridY < i.GridY).OrderBy(x => x.GridY).LastOrDefault();
            if (road3 != null && road3.GridY + (road3.Bound.y * 0.5f + Scale * 0.5f) == i.GridY) i.roads[3] = road3;
        }
        foreach (var i in intersections)
        {
            i.FindNodes();
            i.ConnectNodes();
        }
        foreach (var vehicle in vehicles) vehicle.Init(roads);
    }
}

public class IntersectionInfo
{
    public List<Vehicle> vehicles = new List<Vehicle>();
    public bool isOccupied;
}