using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSPlayerController : MonoBehaviour
{
  [SerializeField] float speed = 20f;
  [SerializeField] float jumpHeight = 0.4f;
  [SerializeField] float gravity = 9.81f;
  [SerializeField] float airControl = 10f;
  [SerializeField] float CrouchSpeed = 5f;
  [SerializeField] float crouchDepthRatio = 0.5f;

  public bool IsCrouching { get; private set; }

  private Vector3 input;
  private Vector3 moveDirection;
  private CharacterController controller;
  private Vector3 ogScale;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    controller = GetComponent<CharacterController>();
    ogScale = transform.localScale;
  }

  // Update is called once per frame
  void Update()
  {
    float moveHorizonatal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");
    input = transform.right * moveHorizonatal + transform.forward * moveVertical;
    input.Normalize();

    HandleJump();
    HandleCrouch();

    moveDirection.y -= gravity * Time.deltaTime;
    controller.Move(speed * Time.deltaTime * moveDirection);
  }

  void HandleJump()
  {
    if (controller.isGrounded)
    {
      moveDirection = input;

      if (Input.GetButton("Jump"))
      {
        moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
      }
      else
      {
        moveDirection.y = 0.0f;
      }
    }
    else
    {
      input.y = moveDirection.y;
      moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
    }
  }

  void HandleCrouch()
  {
    bool crouch = Input.GetButton("Crouch");
    IsCrouching = crouch;

    Crouch();
  }

  void Crouch()
  {
    float step = CrouchSpeed * Time.deltaTime;

    Vector3 targetScale = transform.localScale;
    targetScale.y = IsCrouching ? ogScale.y * crouchDepthRatio : ogScale.y;

    transform.localScale = Vector3.Lerp(transform.localScale, targetScale, step);
  }
}
