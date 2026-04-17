using UnityEngine;

public class TableInteraction : MonoBehaviour
{
    public Transform sitTarget;
    public ParticleSystem tableParticles;
    public float transitionSpeed = 3f;
    public KeyCode exitKey = KeyCode.E;

    private Camera cam;
    private bool isSitting = false;
    private Transform originalParent;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Renderer playerRenderer;

    public void Interact(Camera cam)
    {
        if (isSitting) return;
        this.cam = cam;
        isSitting = true;

        originalParent = cam.transform.parent;
        originalPos = cam.transform.localPosition;
        originalRot = cam.transform.localRotation;

        playerRenderer = cam.transform.root.GetComponentInChildren<Renderer>();
        if (playerRenderer != null) playerRenderer.enabled = false;

        cam.transform.SetParent(null);

        var controller = originalParent.GetComponent<FPSPlayerController>();
        if (controller) controller.enabled = false;

        if (tableParticles != null)
        {
            tableParticles.Stop();
            tableParticles.Clear();
        }
    }

    void Update()
    {
        if (!isSitting || cam == null) return;

        cam.transform.position = Vector3.Lerp(cam.transform.position, sitTarget.position, Time.deltaTime * transitionSpeed);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, sitTarget.rotation, Time.deltaTime * transitionSpeed);

        if (Input.GetKeyDown(exitKey))
            StopSitting();
    }

    void StopSitting()
    {
        isSitting = false;

        cam.transform.SetParent(originalParent);
        cam.transform.localPosition = originalPos;
        cam.transform.localRotation = originalRot;

        playerRenderer = cam.transform.root.GetComponentInChildren<Renderer>();
        playerRenderer.enabled = true;

        var controller = originalParent.GetComponentInParent<FPSPlayerController>();
        if (controller) controller.enabled = true;

        cam = null;
    }
}