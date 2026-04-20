using UnityEngine;
using UnityEngine.AI;

public class ExBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    GameObject prometheus;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        prometheus = GameObject.FindGameObjectWithTag("Prometheus");

        if (!player)
        {
            Debug.LogWarning("No player found in ExBehavior.cs");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!prometheus.GetComponent<Prometheus>().IsDriven) return;

        Drive();
    }

    void Drive()
    {
        agent.SetDestination(player.transform.position);
    }
}
