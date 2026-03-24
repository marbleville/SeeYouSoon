using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterLevelStart();
        }
    }

    public void HandleLevelFail()
    {
        Debug.Log("Level failed");
    }

    public void HandleLevelWin()
    {
        Debug.Log("Level won");
    }
}