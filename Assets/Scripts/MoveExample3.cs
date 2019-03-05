using UnityEngine;

public class MoveExample3 : MonoBehaviour
{
    public const float CurveDistance = 4;
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
        var roads = FindObjectsOfType<Road>();
        var intersections = FindObjectsOfType<Intersection>();
        vehicles = FindObjectsOfType<Vehicle>();
        foreach (var road in roads) road.FindNodes();
        foreach (var i in intersections)
        {
            i.FindNodes();
            i.ConnectNodes();
        }
        foreach (var vehicle in vehicles) vehicle.Init(roads);
    }
}
