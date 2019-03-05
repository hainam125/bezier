using System.Linq;
using UnityEngine;

public class MoveExample3 : MonoBehaviour
{
    public const float CurveDistance = 4;
    public const float Scale = 4;
    private Vehicle[] vehicles;
    
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        for(int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].UpdateGame(deltaTime);
        }
    }

    private void Init()
    {
        vehicles = FindObjectsOfType<Vehicle>();
        var roads = FindObjectsOfType<Road>();
        var intersections = FindObjectsOfType<Intersection>();
        foreach (var road in roads) road.Init();
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
