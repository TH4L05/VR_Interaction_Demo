/// <author>Thomas Krahl</author>

using UnityEngine;
using eecon_lab.Input;
using System.Collections;


namespace eecon_lab.Movement.MouseAndKeyboard
{
    public class PlayerMovement : MonoBehaviour
    {
        #region SerializedFields
     
        public bool isEnabled;
        [Space(2f)]
        private Transform mainTransform;
        [SerializeField] private Transform rotationPivot;
        [SerializeField] private MovementData movementData;

        [SerializeField, Range(1.0f, 2.5f)] private float cameraHeight = 1.77f;
        [SerializeField, Range(1.0f, 100.0f)] private float rotationSensitivity = 10.0f;

        [Header("Options")]
        [SerializeField] private bool canMove = true;
        [SerializeField] private bool canRotate = true;
        [SerializeField] private bool canSprint;
        [SerializeField] private bool canJump;
        [SerializeField] private bool canSlide;
   
        [Header("Audio")]
        private float accumulated_Distance = 1f;
        private float step_Distance = 0f;

        #endregion

        #region PrivateFields

        private CharacterController characterController;
        private Vector2 movementInput;
        private Vector2 mouseInputValue;

        private bool useVR;
        private bool isGrounded;
        private bool jump;
        private bool sprint;

        private Vector3 momentum = Vector3.zero;
        private Vector3 groundVelocity = Vector3.zero;
        private Vector3 verticalVelocity = Vector3.zero;
        private Vector3 characterVelocity = Vector3.zero;
        private Vector3 lastmovement = Vector3.zero;
        private float xRotation = 0f;
        private float speed = 1f;
        private float lastspeed = 1f;
        private float drag;
        private Coroutine coroutineRotate;
        private static float teleportLastActiveTime;
        private Vector3 SlopeHitNormal;
        private bool isSliding;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            GetComponents();
        }

        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            if (!isEnabled) return;
            ReadInputValues();
            UpdateMovementState();         
        }

        #endregion

        #region Setup

        private void ReadInputValues()
        {
            movementInput = InputHandler.Instance.MovementAxisInputValue;
            mouseInputValue = InputHandler.Instance.MouseAxisInputValue;
        }

        private void GetComponents()
        {
            mainTransform = transform.parent;
            characterController = GetComponentInParent<CharacterController>();
        }

        private void Setup()
        {
            useVR = Game.Instance.VRactive;
            rotationPivot.localPosition = new Vector3(rotationPivot.localPosition.x, cameraHeight, rotationPivot.localPosition.z);
            speed = movementData.move_speed;
            step_Distance = movementData.stepDistanceWalk;
        }

        #endregion

        

        private void UpdateMovementState()
        {
            bool wasGrounded = isGrounded;
            GroundCheck();  
            //SlideCheck();

            if (canJump) JumpCheck();
            if (canSprint) SprintOnInput();

            if (canMove) UpdateMovement();
            if (canRotate) UpdateRotation();
        }

        private void UpdateMovement()
        {
            if (useVR)
            {
                UpdateMovementVR();
                return;
            }

            Vector3 horizontalVelocity = mainTransform.right * movementInput.x + mainTransform.forward * movementInput.y;

            if (!isGrounded)
            {
                characterVelocity = groundVelocity;
            }

            characterVelocity = horizontalVelocity * speed;
            characterVelocity.y = verticalVelocity.y;

            /*if (canSlide && isSliding)
            {
                //slide
                characterVelocity += new Vector3(SlopeHitNormal.x, -SlopeHitNormal.y, SlopeHitNormal.z) * movementData.slide_speed;
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
            if (momentum.magnitude >= 0f)
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
                }
            }

            if (momentum.magnitude < 0.1f)
            {
                momentum = Vector3.zero;
            }         
        }

        private void UpdateMovementVR()
        {
            Vector3 direction = Vector3.zero;

            bool forward = InputHandler.Instance.GetMoveForwardState();
            bool backward = InputHandler.Instance.GetMoveBackwardState();

            if (backward)
            {              
                direction = -mainTransform.forward;
            }
            else if (forward)
            {
                direction = mainTransform.forward;
            }
            Vector3 motion = Time.deltaTime * speed * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0f, 9.81f, 0f) * Time.deltaTime;
            characterController.Move(motion);
        }

        private void UpdateRotation()
        {
            if (useVR)
            {
                UpdateRotationVR();
                return;
            }

            var rotations = mouseInputValue * rotationSensitivity * Time.deltaTime;
            xRotation -= rotations.y;
            xRotation = Mathf.Clamp(xRotation, -85f, 85f);
            Vector3 targetRotation = mainTransform.eulerAngles;
            targetRotation.x = xRotation;
            mainTransform.Rotate(Vector3.up, rotations.x);
            rotationPivot.eulerAngles = targetRotation;         
        }

        private void UpdateRotationVR()
        {

            if (Time.time < (teleportLastActiveTime + movementData.canTurnEverySeconds))  return;

            if (coroutineRotate != null)
            {
                //if (director != null && director.state == PlayState.Playing) director.Stop();
                StopCoroutine(coroutineRotate);
            }

            float angle = 0f;
            /*if (rotateLeft.state)
            {
                angle = -movementData.snapAngle;
            }
            else if (rotateRight.state)
            {
                angle = movementData.snapAngle;
            }*/
            coroutineRotate = StartCoroutine(DoPlayerRotation(angle));
        }

        private IEnumerator DoPlayerRotation(float angle)
        {
            canRotate = false;
            yield return new WaitForSeconds(0.5f);

            mainTransform.Rotate(Vector3.up, angle);

            float startTime = Time.time;
            float endTime = startTime + movementData.canTurnEverySeconds;

            while (Time.time <= endTime)
            {
                yield return null;
            };
            canRotate = true;

        }

        private void JumpCheck()
        {
            if (InputHandler.Instance.JumpInputValue && !jump)
            {
                jump = true;
                verticalVelocity.y = Mathf.Sqrt(-2 * movementData.jump_force * -movementData.gravity);
                drag = movementData.drag;
                momentum = lastmovement;
            }
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
                jump = false;
                drag = movementData.drag;
                lastspeed = speed;               
            }
            else
            {
                isGrounded = false;
                verticalVelocity.y += -movementData.gravity * Time.deltaTime;
                lastspeed = speed;
            }
        }

        private void SlideCheck()
        {
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                SlopeHitNormal = slopeHit.normal;
                if (Vector3.Angle(slopeHit.normal, Vector3.up) > characterController.slopeLimit)
                {
                    isSliding = true;
                }
            }
            else
            {
                isSliding = false;
            }
        }
    }
}

