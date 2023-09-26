/// <author>Thomas Krahl</author>

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace eecon_lab.Movement.VR
{
    public class PlayerMovement : MonoBehaviour
    {
        #region SerializedFields

        [Header("Input")]
        [SerializeField] private SteamVR_Action_Boolean moveForward;
        [SerializeField] private SteamVR_Action_Boolean moveBackward;
        [SerializeField] private SteamVR_Action_Boolean rotateLeft;
        [SerializeField] private SteamVR_Action_Boolean rotateRight;
        [SerializeField] private SteamVR_Action_Boolean snapLeftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
        [SerializeField] private SteamVR_Action_Boolean snapRightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");

        [Header("Settings")]
        [SerializeField] private bool movementEnabled = true;
        [SerializeField] private bool autoMovement = false;
        [SerializeField] private float movementSpeed = 1f;
        [SerializeField] private float movementSpeedAuto = 1f;
        //[SerializeField] private float rotationSensitivity = 10f;
        //[SerializeField] private bool fadeScreen = true;
        //[SerializeField] private Color screenFadeColor = Color.black;
        [SerializeField] float snapAngle = 30.0f;

        [SerializeField] private float canTurnEverySeconds = 0.4f;
        [SerializeField] private PlayableDirector director;

        #endregion

        #region PrivateFields

        private Coroutine rotateCoroutine;
        private CharacterController characterController;
        private static float teleportLastActiveTime;
        private bool canRotate = true;

        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
        }

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                throw new NullReferenceException("CharacterController Component is Missing");
            }
        }

        void Update()
        {
            if (!movementEnabled) return;
            SnapTurn();
        }

        private void FixedUpdate()
        {
            if (!movementEnabled) return;
            Movement();
            if (!autoMovement) return;
            AutoMovement();
            //Rotation();       
        }

        #endregion

        #region Movement

        private void Movement()
        {
            if (autoMovement) return;
            Vector3 direction = Vector3.zero;

            if (moveForward.state)
            {
                direction = Player.instance.hmdTransform.TransformDirection(new Vector3(0, 0f, 1f));
            }
            else if (moveBackward.state)
            {
                direction = Player.instance.hmdTransform.TransformDirection(new Vector3(0, 0f, -1f));
            }
            Vector3 motion = Time.deltaTime * movementSpeed * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0f, 9.81f, 0f) * Time.deltaTime;
            characterController.Move(motion);
        }

        private void AutoMovement()
        {
            Vector3 direction = transform.forward;
            Vector3 motion = Time.deltaTime * movementSpeedAuto * direction - new Vector3(0f, 9.81f, 0f) * Time.deltaTime;
            characterController.Move(motion);
        }

        /*private void Rotation()
        {
            Vector2 input = Vector2.zero;

            if (rotateLeft.state)
            {
                input = new Vector2(1, 0);
            }
            else if (rotateRight.state)
            {
                input = new Vector2(-1, 0);
            }
            float rotationAngle = input.x * rotationSensitivity * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAngle);
        }*/

        private void SnapTurn()
        {
            if (autoMovement) return;
            Player player = Player.instance;
            if (canRotate && snapLeftAction != null && snapRightAction != null && snapLeftAction.activeBinding && snapRightAction.activeBinding)
            {
                //only allow snap turning after a quarter second after the last teleport
                if (Time.time < (teleportLastActiveTime + canTurnEverySeconds))
                    return;

                // only allow snap turning when not holding something

                bool rightHandValid = player.rightHand.currentAttachedObject == null ||
                    (player.rightHand.currentAttachedObject != null
                    && player.rightHand.currentAttachedTeleportManager != null
                    && player.rightHand.currentAttachedTeleportManager.teleportAllowed);

                bool leftHandValid = player.leftHand.currentAttachedObject == null ||
                    (player.leftHand.currentAttachedObject != null
                    && player.leftHand.currentAttachedTeleportManager != null
                    && player.leftHand.currentAttachedTeleportManager.teleportAllowed);


                bool leftHandTurnLeft = snapLeftAction.GetStateDown(SteamVR_Input_Sources.LeftHand) && leftHandValid;
                bool rightHandTurnLeft = snapLeftAction.GetStateDown(SteamVR_Input_Sources.RightHand) && rightHandValid;

                bool leftHandTurnRight = snapRightAction.GetStateDown(SteamVR_Input_Sources.LeftHand) && leftHandValid;
                bool rightHandTurnRight = snapRightAction.GetStateDown(SteamVR_Input_Sources.RightHand) && rightHandValid;

                if (leftHandTurnLeft || rightHandTurnLeft)
                {
                    RotatePlayer(-snapAngle);
                }
                else if (leftHandTurnRight || rightHandTurnRight)
                {
                    RotatePlayer(snapAngle);
                }
            }
        }

        public void RotatePlayer(float angle)
        {
            if (rotateCoroutine != null)
            {
                if (director != null && director.state == PlayState.Playing) director.Stop();
                StopCoroutine(rotateCoroutine);
            }

            if (/*fadeScreen &&*/ director != null) director.Play();
            rotateCoroutine = StartCoroutine(DoRotatePlayer(angle));
        }

        private IEnumerator DoRotatePlayer(float angle)
        {
            Player player = Player.instance;

            canRotate = false;
            yield return new WaitForSeconds((float)director.playableAsset.duration / 2);

            Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
            player.trackingOriginTransform.position -= playerFeetOffset;
            player.transform.Rotate(Vector3.up, angle);
            playerFeetOffset = Quaternion.Euler(0.0f, angle, 0.0f) * playerFeetOffset;
            player.trackingOriginTransform.position += playerFeetOffset;

            float startTime = Time.time;
            float endTime = startTime + canTurnEverySeconds;

            while (Time.time <= endTime)
            {
                yield return null;
            };
            canRotate = true;
        }

        #endregion
    }
}

