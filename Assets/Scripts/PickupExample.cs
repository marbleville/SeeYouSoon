using UnityEngine;

public class PickupExample : APickupable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void OnPickup()
    {
        Debug.Log("Picked Up!");
    }

    public override void OnDrop()
    {
        Debug.Log("Dropped!");
    }
}
