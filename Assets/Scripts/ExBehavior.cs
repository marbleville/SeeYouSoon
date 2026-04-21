using UnityEngine;
using UnityEngine.AI;

public class ExBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    GameObject prometheus;
    Vector3 lastPlayerPos;
    float timeSinceLastPos;
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
        timeSinceLastPos += Time.deltaTime;

        if (timeSinceLastPos > 3)
        {
            lastPlayerPos = player.transform.position;
            timeSinceLastPos = 0;
        }

        if (!prometheus.GetComponent<Prometheus>().IsDriven) return;

        Drive();
    }

    void Drive()
    {
        if (Vector3.Distance(player.transform.position, lastPlayerPos) < 0.1)
        {
            agent.SetDestination(gameObject.transform.position + (player.transform.forward * -5));
            return;
        }

        agent.SetDestination(player.transform.position);
    }
}
