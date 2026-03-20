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
  [SerializeField] float sprintSpeed = 30f;

  public bool IsCrouching { get; private set; } = false;
  public bool IsSprinting { get; private set; } = false;

  private Vector3 input;
  private Vector3 moveDirection;
  private CharacterController controller;
  private Vector3 originalScale;
  private float originalSpeed;
  private float targetSpeed;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    controller = GetComponent<CharacterController>();
    originalScale = transform.localScale;
    originalSpeed = speed;
    targetSpeed = speed;
  }

  // Update is called once per frame
  void Update()
  {
    float moveHorizonatal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");
    input = transform.right * moveHorizonatal + transform.forward * moveVertical;
    input.Normalize();

    targetSpeed = originalSpeed;

    // Movement handlers go here
    HandleJump();
    HandleCrouch();
    HandleSprint();
    // End movement handlers

    speed = targetSpeed;

    moveDirection.y -= gravity * Time.deltaTime;
    controller.Move(speed * Time.deltaTime * moveDirection);

    Debug.Log(speed);
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
    UpdateSpeed(IsCrouching, originalSpeed * crouchDepthRatio);

    float step = CrouchSpeed * Time.deltaTime;

    Vector3 targetScale = transform.localScale;
    targetScale.y = IsCrouching ? originalScale.y * crouchDepthRatio : originalScale.y;

    transform.localScale = Vector3.Lerp(transform.localScale, targetScale, step);
  }

  void HandleSprint()
  {
    bool sprint = Input.GetButton("Sprint");
    IsSprinting = sprint && !IsCrouching;

    Sprint();
  }

  void Sprint()
  {
    // TODO: Increase FOV while sprinting
    UpdateSpeed(IsSprinting, sprintSpeed);
  }

  void UpdateSpeed(bool pred, float speed)
  {
    targetSpeed = pred ? speed : targetSpeed;
  }
}
