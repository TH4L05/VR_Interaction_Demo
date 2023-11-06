/// <author>Thomas Krahl</author>

using UnityEngine;
using eecon_lab.Input;


namespace eecon_lab.Movement.MouseAndKeyboard
{
    public class PlayerMovementMK : MonoBehaviour
    {
        [SerializeField] private Transform rotationPivot;
        [SerializeField] private MovementData movementData;
        [SerializeField] private float rotationSensitivity = 10f;

        [Header("Options")]
        [SerializeField] private bool canMove = true;
        [SerializeField] private bool canRotate = true;
        [SerializeField] private bool canSprint;
        [SerializeField] private bool canJump;
        [SerializeField] private bool canSlide;

        /*[Header("Audio")]
        private float accumulated_Distance = 1f;
        private float step_Distance = 0f;
        [SerializeField] private float walk_step_Distance = 1f;
        [SerializeField] private float sprint_step_Distance = 0.5f;
        [SerializeField] private float crouch_step_Distance = 1.5f;*/

        private CharacterController characterController;

        private Vector2 movementInput;
        private Vector2 mouseInputValue;

        private bool isGrounded;
        private bool jump;
        private bool jumpLastFrame;
        private bool jumpReady;
        private bool sprint;

        private Vector3 momentum = Vector3.zero;
        private Vector3 verticalVelocity = Vector3.zero;
        private Vector3 characterVelocity = Vector3.zero;
        private Vector3 lastmovement = Vector3.zero;
        private float xRotation = 0f;
        private float speed = 1f;
        private float lastspeed = 1f;
        private float drag;

        private void Awake()
        {
            GetComponents();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            speed = movementData.move_speed;
            //step_Distance = walk_step_Distance;
        }

        private void Update()
        {
            ReadInputValues();
            UpdateMovementState();
         
        }

        private void ReadInputValues()
        {
            movementInput = InputHandler.Instance.MovementAxisInputValue;
            mouseInputValue = InputHandler.Instance.MouseAxisInputValue;
        }

        private void GetComponents()
        {
            characterController = GetComponent<CharacterController>();
        }
     
        private void UpdateMovementState()
        {
            bool wasGrounded = isGrounded;
            GroundCheck();

            /*if (isGrounded && !wasGrounded)
            {
                //landingFrame = true;                          
                verticalVelocity.y = -1;
                jumpReady = true;
            }
            if (wasGrounded && !isGrounded)
            {
                //firstAerialFrame = true;
            }*/

            //SlideCheck();

            if (canJump) JumpCheck();
            if (canSprint) SprintOnInput();

            if (canMove) UpdateMovement();
            if (canRotate) UpdateRotation();
        }

        private void UpdateMovement()
        {
            Vector3 horizontalVelocity = transform.right * movementInput.x + transform.forward * movementInput.y;

                if (isGrounded)
                {
                    characterVelocity = horizontalVelocity * speed;
                }
                /*else
                {
                    if (Time.timeScale > 0)
                    {
                        momentum += horizontalVelocity * movementData.aerial_control;
                    }
                }*/

                /*if (landingFrame)
                {
                    momentum -= horizontalVelocity * speed;
                }

                if (jumpLastFrame || firstAerialFrame)
                {
                    characterVelocity = Vector3.zero;
                    momentum.x = lastmovement.x; momentum.z = lastmovement.z;
                }*/

                characterVelocity.y = verticalVelocity.y;

                /*if (canSlide && isSliding)
                {
                    //slide
                    characterVelocity += new Vector3(SlopeHitNormal.x, -SlopeHitNormal.y, SlopeHitNormal.z) * characterData.slide_speed;
                }
                else
                {
                    //Add momentum
                    characterVelocity += momentum;
                }*/
             
                lastmovement = characterVelocity;
                characterVelocity *= Time.deltaTime;
                characterController.Move(characterVelocity);
               
                //Damp momentum
                /*if (momentum.magnitude >= 0f)
                {
                    //prevent character from sliding on the ground for too long
                    if (isGrounded)
                    {
                        momentum.y = 0f;
                        momentum -= momentum * movementData.groundfriction * Time.deltaTime;
                 
                        if ((horizontalVelocity * speed).magnitude > momentum.magnitude)
                        {
                            momentum = Vector3.zero;
                        }
                    }
                    else
                    {
                        momentum -= momentum * drag * Time.deltaTime;
                        //Level.instance.audioEvents.PlayAudioEvent("PlayerSlideStop", gameObject);
                    }
                }

                if (momentum.magnitude < 0.1f)
                {
                    momentum = Vector3.zero;
                }*/
            
        }

        private void UpdateRotation()
        {
            var rotations = mouseInputValue * rotationSensitivity * Time.deltaTime;
            xRotation -= rotations.y;
            xRotation = Mathf.Clamp(xRotation, -85f, 85f);
            Vector3 targetRotation = transform.eulerAngles;
            targetRotation.x = xRotation;
            transform.Rotate(Vector3.up, rotations.x);
            rotationPivot.eulerAngles = targetRotation;         
        }

        private void JumpCheck()
        {
            if (isGrounded && InputHandler.Instance.JumpInputValue)
            {
                verticalVelocity.y = Mathf.Sqrt(-2 * movementData.jump_force * -movementData.gravity);
                drag = movementData.drag;
                momentum = lastmovement;
            }

            /*if (jumpReady)
            {
                if (Time.time - lastGroundedTime <= movementData.coyoteTimeframe)
                {
                    if (Time.time - jumpButtonPressedTime <= movementData.jumpBufferTimeframe)
                    {
                        jumpLastFrame = true;
                        //lastGroundedTime = null;
                        //jumpButtonPressedTime = null;
                        jumpReady = false;
                        drag = movementData.drag;
                        momentum = lastmovement;
                        verticalVelocity.y = Mathf.Sqrt(-2 * movementData.jump_force * -movementData.gravity);                       
                    }
                }
            }*/
        }

        private void SprintOnInput()
        {
            if (sprint)
            {
                speed = movementData.sprint_speed;
            }
            else
            {
                speed = movementData.move_speed;
            }
        }

        private void GroundCheck()
        {
            if (characterController.isGrounded)
            {
                isGrounded = true;
                //lastGroundedTime = Time.time;
                //drag = movementData.drag;
                //lastspeed = speed;               
            }
            else
            {
                isGrounded = false;
                verticalVelocity.y += -movementData.gravity * Time.deltaTime;
                //lastspeed = speed;
            }
        }

        /*private void SlideCheck()
        {
            if (playerController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                SlopeHitNormal = slopeHit.normal;
                if (Vector3.Angle(slopeHit.normal, Vector3.up) > playerController.slopeLimit)
                {
                    isSliding = true;
                }
            }
            else
            {
                isSliding = false;
            }
        }*/
    }
}

