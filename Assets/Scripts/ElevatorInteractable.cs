using UnityEngine;

public class ElevatorInteractable : AInteractable
{

    new void Start()
    {
        base.Start();
    }

  // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    public override void OnInteract()
    {
        LevelManager levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager)
        {
            levelManager.HandleLevelWin();
            levelManager.LoadNextLevel();
        }
    }
}
