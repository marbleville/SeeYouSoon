using UnityEngine;

public class TableInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
    
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            TableInteraction table = hit.collider.GetComponent<TableInteraction>();
            if (table != null)
            {
                if (Input.GetKeyDown(interactKey))
                    table.Interact(playerCamera);
            }
        }
    }
}