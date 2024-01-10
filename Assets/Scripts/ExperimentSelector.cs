

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

namespace eecon_lab.Experiment
{
    public class ExperimentSelector : MonoBehaviour
    {
        [System.Serializable]
        public class ExperimentSelection
        {
            [Header("Options")]
            public bool isEnabled;
            public string name;
            [HideInInspector]public bool isActive;
            public bool keepPreviousActiveEnabled;
            public bool changePlayerPosition;

            [Space(2f),Header("Setup"), Space(5f)]
            public InputAction inputAction;
            [Space(10f)] public Vector3 playerPosition;
            [Space(5f)] public GameObject playerPositionObject;
            //public GameObject parentObject;
            [Space(10f)] public GameObject[] objectSelection;
            public Vector3 scale = Vector3.one;
            public bool usePlayableDirector = false;

        }

        #region SerializedFields

        [SerializeField] private List<ExperimentSelection> experimentSelections = new List<ExperimentSelection>();
        [SerializeField] private List<InputAction> inputActions = new List<InputAction>();
        [SerializeField] private PlayableDirector director;
        
        private bool onChange;

        #endregion

        #region PrivateFields

        private int activeIndex;
        private InputAction lastInputAction;

        #endregion

        private void Start()
        {
            foreach (var selection in experimentSelections)
            {
                if (!selection.isEnabled || selection.inputAction == null) continue;
                inputActions.Add(selection.inputAction);
            }

            foreach (var inputAction in inputActions)
            {
                inputAction.Enable();
            }
        }

        private void LateUpdate()
        {
            if (onChange) return;
            foreach (var inputAction in inputActions)
            {
                if (inputAction.WasPressedThisFrame())
                {
                    onChange = true;
                    lastInputAction = inputAction;
                    if (activeIndex == GetListIndex(lastInputAction))
                    {
                        Debug.Log("Same Selection");
                        onChange = false;
                        return;
                    }

                    if(director != null && director.state == PlayState.Playing) director.Stop();
                    if (experimentSelections[activeIndex].usePlayableDirector)
                    {
                        director?.Play();
                    }
                    else
                    {
                        DoSelection();
                    }

                    break;
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var inputAction in inputActions)
            {
                inputAction.Disable();
            }
        }

        public void DoSelection()
        {
            int index = GetListIndex(lastInputAction);
            if (index == -1 || !experimentSelections[index].isEnabled)
            {
                onChange = false;
                return;
            }
  
            if (!experimentSelections[index].keepPreviousActiveEnabled) DisableActiveSelection();
            activeIndex = index;

            if (experimentSelections[index].changePlayerPosition)
            {
                if (experimentSelections[index].playerPosition != Vector3.zero)
                {
                    Game.Instance.Teleport.TeleportPlayerCustom(experimentSelections[index].playerPosition);
                }
                else if (experimentSelections[index].playerPositionObject != null)
                {
                    Game.Instance.Teleport.TeleportPlayerCustom(experimentSelections[index].playerPositionObject.transform.position);
                }               
            }

            if (experimentSelections[index].objectSelection.Length > 0)
            {
                foreach (var gameObject in experimentSelections[index].objectSelection)
                {
                    gameObject.SetActive(true);
                    gameObject.transform.localScale = experimentSelections[index].scale;
                }
            }                 
            onChange = false;
        }

        private int GetListIndex(InputAction inputAction)
        {
            int index = -1;
            for (int i = 0; i < experimentSelections.Count; i++)
            {
                if (experimentSelections[i].inputAction == inputAction)
                {
                    index = i;
                    return index;
                }
            }
            return index;
        }

        private void DisableActiveSelection()
        {
            if (activeIndex == -1) return;
            if (experimentSelections.Count == 1) return;

            GameObject[] objects = experimentSelections[activeIndex].objectSelection;
            if (objects.Length < 1) return;

            foreach (var gameObject in objects)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

