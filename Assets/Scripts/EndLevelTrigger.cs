using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PrometheusCollider")) return;

        LevelManager levelManager = FindFirstObjectByType<LevelManager>();
        levelManager.HandleLevelWin();
    }
}
