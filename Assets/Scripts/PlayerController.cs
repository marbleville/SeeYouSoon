using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FPSPlayerController : MonoBehaviour
{
  [SerializeField] float speed = 5f;
  [SerializeField] float jumpHeight = 0.4f;
  [SerializeField] float gravity = 9.81f;
  [SerializeField] float airControl = 10f;
  [SerializeField] float CrouchSpeed = 5f;
  [SerializeField] float crouchDepthRatio = 0.5f;
  [SerializeField] float sprintSpeed = 10f;

  [Header("Footsteps")]
  [SerializeField] AudioClip leftFootClip;
  [SerializeField] AudioClip rightFootClip;
  [SerializeField] float walkStepInterval = 0.5f;
  [SerializeField] float sprintStepInterval = 0.35f;
  [SerializeField] float crouchStepInterval = 0.65f;
  [SerializeField] float stepRateMultiplier = 1.2f;
  [SerializeField] float walkStepVolume = 1f;
  [SerializeField] float sprintStepVolume = 1f;
  [SerializeField] float crouchStepVolume = 0.45f;
  [SerializeField] float footstepStartMoveSpeed = 0.35f;
  [SerializeField] float footstepStopMoveSpeed = 0.2f;
  [SerializeField] float footstepRetriggerDelay = 0.1f;
  [SerializeField] float minStepInterval = 0.16f;
  [SerializeField] float maxStepInterval = 0.75f;
  [SerializeField] float crouchToggleStepDelay = 0.12f;

  public bool IsCrouching { get; private set; } = false;
  public bool IsSprinting { get; private set; } = false;

  private Vector3 input;
  private Vector3 moveDirection;
  private CharacterController controller;
  private AudioSource audioSource;
  private Vector3 originalScale;
  private float originalSpeed;
  private float targetSpeed;

  private float footstepTimer;
  private Vector3 lastPosition;
  private bool playLeftNext = true;
  private bool wasFootstepMoving = false;
  private bool previousCrouchState = false;

  void Start()
  {
    controller = GetComponent<CharacterController>();
    audioSource = GetComponent<AudioSource>();

    originalScale = transform.localScale;
    originalSpeed = speed;
    targetSpeed = speed;
    lastPosition = transform.position;

    audioSource.playOnAwake = false;
    audioSource.loop = false;
  }

  void Update()
  {
    if (DialogueManager.Instance && DialogueManager.Instance.IsDialogueActive())
    {
      StopFootsteps();
      return;
    }

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

    if (controller.enabled)
    {
      controller.Move(speed * Time.deltaTime * moveDirection);
    }

    HandleFootsteps();
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

    if (previousCrouchState != IsCrouching)
    {
      // Prevent bursty first steps when crouch state toggles
      footstepTimer = Mathf.Max(footstepTimer, crouchToggleStepDelay);
      lastPosition = transform.position;
      wasFootstepMoving = false;
    }
    previousCrouchState = IsCrouching;

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

  // Plays footstep sounds based on real movement speed
  void HandleFootsteps()
  {
    Vector3 horizontalDelta = transform.position - lastPosition;
    horizontalDelta.y = 0f;

    // gives movement distance, turned into speed by dividing by frame time
    float actualMoveSpeed = horizontalDelta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
    lastPosition = transform.position;

    // checks if there is movement input without doing a square root
    bool wantsToMove = input.sqrMagnitude > 0.01f;
    bool canStep = controller.isGrounded && wantsToMove;
    float moveThreshold = wasFootstepMoving ? footstepStopMoveSpeed : footstepStartMoveSpeed;
    bool isActuallyMoving = canStep && actualMoveSpeed > moveThreshold;

    if (!isActuallyMoving)
    {
      wasFootstepMoving = false;

      // Keeps the bigger timer so footstep sounds do not spam.
      footstepTimer = Mathf.Max(footstepTimer, footstepRetriggerDelay);
      return;
    }

    wasFootstepMoving = true;

    footstepTimer -= Time.deltaTime;
    if (footstepTimer > 0f) return;

    PlayFootstepClip();
    footstepTimer = GetStepInterval(actualMoveSpeed);
  }

  // Chooses how long to wait before the next footstep sound
  float GetStepInterval(float actualMoveSpeed)
  {
    float baseInterval = walkStepInterval;
    if (IsSprinting) baseInterval = sprintStepInterval;
    if (IsCrouching) baseInterval = crouchStepInterval;

    // converts movement speed into a scale w minimum
    float speedScale = Mathf.Max(actualMoveSpeed / Mathf.Max(originalSpeed, 0.01f), 0.2f);

    // Faster speed = shorter delay between footsteps
    float adjustedInterval = baseInterval / (speedScale * Mathf.Max(stepRateMultiplier, 0.01f));

    // Keeps the final delay inside safe min/max range
    return Mathf.Clamp(adjustedInterval, minStepInterval, maxStepInterval);
  }

  void PlayFootstepClip()
  {
    AudioClip clipToPlay = playLeftNext ? leftFootClip : rightFootClip;
    if (clipToPlay == null) return;

    float volume = walkStepVolume;
    if (IsSprinting) volume = sprintStepVolume;
    if (IsCrouching) volume = crouchStepVolume;

    audioSource.PlayOneShot(clipToPlay, volume);
    playLeftNext = !playLeftNext;
  }

  void StopFootsteps()
  {
    footstepTimer = 0f;
  }
}
