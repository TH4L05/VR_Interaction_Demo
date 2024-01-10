/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Valve.VR;

namespace eecon_lab.Input
{
    public class InputHandler : MonoBehaviour
    {
        #region SerializedField

        [SerializeField] bool enableMovmentInput;
        [SerializeField] bool enableOtherInput;

        [Header("VR Input")]
        [SerializeField] private SteamVR_Action_Boolean moveForward;
        [SerializeField] private SteamVR_Action_Boolean moveBackward;
        [SerializeField] private SteamVR_Action_Boolean rotateLeft;
        [SerializeField] private SteamVR_Action_Boolean rotateRight;

        #endregion

        #region PrivateFields

        private GameControls gameControls;
        private List<InputAction> inputActionsMovement;
        private List<InputAction> inputActionsOther;

        #endregion

        #region PublicFields

        public static InputHandler Instance { get; private set; }
        public Vector2 MovementAxisInputValue { get; private set; }
        public Vector2 MouseAxisInputValue { get; private set; }
        public bool JumpInputValue { get; private set; }
        public bool SprintInputValue { get; private set; }
        public bool CrouchInputValue { get; private set; }
        public bool ExtraInputValue1 { get; private set; }

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            Intitialize();
        }

        private void Start()
        {
            SetInputActions();
            if(enableMovmentInput) EnableDisableInputActions(true, inputActionsMovement);
            if(enableOtherInput) EnableDisableInputActions(true, inputActionsOther);
        }

        private void OnDestroy()
        {
            EnableDisableInputActions(false, inputActionsMovement);
            EnableDisableInputActions(false, inputActionsOther);
        }

        #endregion

        #region Setup

        private void Intitialize()
        {
            gameControls  = new GameControls();
            inputActionsMovement = new List<InputAction>();
            inputActionsOther = new List<InputAction>();
        }

        private void SetInputActions()
        {
            var inputMovement = gameControls.Movement;
            var inputOther = gameControls.Other;

            inputMovement.BaseMovement.performed += ReadBaseMovementValue;
            inputActionsMovement.Add(inputMovement.BaseMovement);

            inputMovement.Rotation.performed += ReadRotationValue;
            inputActionsMovement.Add(inputMovement.Rotation);

            inputMovement.Jump.performed += JumpInputPerformed;
            inputMovement.Jump.canceled += JumpInputCancelled;
            inputActionsMovement.Add(inputMovement.Jump);


            //------------------------------------------------------------

            inputOther.LoadMainMenu.performed += LoadMainMenu_performed;
            inputOther.LoadMainMenu.canceled += LoadMainMenu_canceled;
            inputActionsOther.Add(inputOther.LoadMainMenu);
        }

        public void EnableDisableInputActions(bool enable, List<InputAction> inputActionList)
        {
            if (enable)
            {
                foreach (InputAction inputAction in inputActionList)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (InputAction inputAction in inputActionList)
                {
                    inputAction.Disable();
                }
            }
        }

        #endregion

        #region Input

        private void LoadMainMenu_canceled(InputAction.CallbackContext context)
        {
            ExtraInputValue1 = context.ReadValueAsButton();
        }

        private void LoadMainMenu_performed(InputAction.CallbackContext context)
        {
            ExtraInputValue1 = context.ReadValueAsButton();
        }

        private void ReadBaseMovementValue(InputAction.CallbackContext context)
        {
            MovementAxisInputValue = context.ReadValue<Vector2>();
        }

        private void ReadRotationValue(InputAction.CallbackContext context)
        {
            MouseAxisInputValue = context.ReadValue<Vector2>();
        }

        private void JumpInputPerformed(InputAction.CallbackContext context)
        {
            JumpInputValue = context.ReadValueAsButton();
        }

        private void JumpInputCancelled(InputAction.CallbackContext context)
        {
            JumpInputValue = context.ReadValueAsButton();
        }

        private void SprintInputPerformed(InputAction.CallbackContext context)
        {
            JumpInputValue = context.ReadValueAsButton();
        }

        private void SprintInputCancelled(InputAction.CallbackContext context)
        {
            JumpInputValue = context.ReadValueAsButton();
        }

        private void CrouchInputPerformed(InputAction.CallbackContext context)
        {
            JumpInputValue = context.ReadValueAsButton();
        }

        private void CrouchInputCancelled(InputAction.CallbackContext context)
        {
            JumpInputValue = context.ReadValueAsButton();
        }

        #endregion

        #region InputVR

        private bool IsRightHandValid()
        {
            return Game.Instance.Player.rightHand.currentAttachedObject != null;
        }

        private bool IsLeftHandValid()
        {
            return Game.Instance.Player.leftHand.currentAttachedObject != null;
        }


        public bool GetMoveForwardState()
        {
            if (moveForward == null || !moveForward.activeBinding) return false;

            bool leftHand =  moveForward.GetState(SteamVR_Input_Sources.LeftHand) && IsLeftHandValid();
            bool rightHand =  moveForward.GetState(SteamVR_Input_Sources.RightHand) &&  IsRightHandValid();

            if (leftHand || rightHand)
            {
                return true;
            }
            return false;
        }

        public bool GetMoveBackwardState()
        {
            if (moveBackward == null || !moveBackward.activeBinding) return false;
            bool leftHand = moveBackward.GetState(SteamVR_Input_Sources.LeftHand) && IsLeftHandValid();
            bool rightHand = moveBackward.GetState(SteamVR_Input_Sources.RightHand) && IsRightHandValid();

            if (leftHand || rightHand)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
