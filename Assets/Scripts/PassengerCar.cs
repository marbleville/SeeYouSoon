using UnityEngine;
using UnityEngine.AI;

public class PassengerCar : MonoBehaviour
{
    public Transform[] waypoints;
    public int startWaypointIdx = 0;

    NavMeshAgent agent;
    int currentWaypointIdx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWaypointIdx = startWaypointIdx;
    }

    // Update is called once per frame
    void Update()
    {
        Drive();
    }

    void Drive()
    {
        if (waypoints.Length == 0) return;

        float waypointDist = Vector3.Distance(waypoints[currentWaypointIdx].position, transform.position);



        if (waypointDist <= 30)
        {
            currentWaypointIdx++;
            currentWaypointIdx = currentWaypointIdx % waypoints.Length;
        }

        agent.SetDestination(waypoints[currentWaypointIdx].position);
    }
}
