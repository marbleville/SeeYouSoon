using UnityEngine;

public class MouseLook : MonoBehaviour
{
  [SerializeField] float mouseSensitivity = 200;
  [SerializeField] float pitchMin = -90f;
  [SerializeField] float pitchMax = 90f;

  Transform playerBody;
  float pitch;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    playerBody = transform.parent.transform;

    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  // Update is called once per frame
  void Update()
  {
    float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

    if (!playerBody) return;

    // yaw at the player 
    playerBody.Rotate(Vector3.up * moveX);

    // pitch at the camera 
    pitch -= moveY;
    pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    transform.localRotation = Quaternion.Euler(pitch, 0, 0);
  }
}
