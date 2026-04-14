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
    mouseSensitivity = GameManager.GetSavedMouseSensitivity();

    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  void OnEnable()
  {
    GameManager.OnMouseSensitivityChanged += ApplySensitivity;
  }

  void OnDisable()
  {
    GameManager.OnMouseSensitivityChanged -= ApplySensitivity;
  }

  // Update is called once per frame
  void Update()
  {
    // When UI menus are open, cursor is unlocked/visible
    // Ignore look input so dragging UI controls does not move camera
    if (Cursor.visible || Cursor.lockState != CursorLockMode.Locked) return;

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

  void ApplySensitivity(float value)
  {
    mouseSensitivity = value;
  }
}
