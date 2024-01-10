/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;

namespace eecon_lab
{
    public class Teleport : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] private bool isEnabled = true;
        [SerializeField] private List<Transform> teleportingPoints = new List<Transform>();

        #endregion

        #region PrivateFields

        private GameObject player;
        private Vector3 teleportPosition = Vector3.zero;
        private int activeIndex = -1;
        private int lastactiveIndex = -1;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            player = Game.Instance.Player.gameObject;
        }

        #endregion

        #region SetTeleportPosition

        public void SetTelportPosition(Vector3 position)
        {
            if (position == Vector3.zero) return;
            teleportPosition = position;
        }

        public void SetTeleportPosition(GameObject obj)
        {
            if (obj == null) return;
            teleportPosition = obj.transform.position;
        }

        public void SetTeleport(int index)
        {
            teleportPosition = teleportingPoints[index].position;
        }

        public void SetTeleport(GameObject gameObject)
        {
            if (gameObject == null || teleportingPoints.Count < 1) return;
            Debug.Log("Set Teleport ...");
            
            int index = 0;
            foreach (var point in teleportingPoints)
            {
                if (point.name == gameObject.name)
                {
                    teleportPosition = point.transform.position;

                    if (activeIndex < 0)
                    {
                        lastactiveIndex = index;
                    }
                    else
                    {
                        lastactiveIndex = activeIndex;
                    }
                    
                    activeIndex = index;
                    return;
                }
                index++;
            }
        }

        #endregion

        #region Teleport

        public void TeleportPlayer()
        {
            if (!isEnabled)
            {
                Debug.LogWarning("INFO - Teleport is NOT enabled");
                return;
            }

            if (teleportPosition == Vector3.zero)
            {
                Debug.LogError("ERROR - Teleport Position is not set");
                return;
            }
            DoTeleport();
        }

        public void TeleportPlayerCustom()
        {
            if (teleportPosition == Vector3.zero)
            {
                Debug.LogError("ERROR - Teleport Position is not set");
                return;
            }
            DoTeleport();
        }

        public void TeleportPlayerCustom(Vector3 teleportPosition)
        {
            if (teleportPosition == Vector3.zero)
            {
                Debug.LogError("ERROR - Teleport Position is not set");
                return;
            }
            this.teleportPosition = teleportPosition;             
            DoTeleport();
        }

        public void TeleportPlayer(Vector3 teleportPosition)
        {
            if (teleportPosition == Vector3.zero)
            {
                Debug.LogError("ERROR - Teleport Position is not set");
                return;
            }
            this.teleportPosition = teleportPosition;         
            DoTeleport();
        }

        private void DoTeleport()
        {
            Debug.Log("Teleporting Player ...");

            EnableATeleportPoint(lastactiveIndex);
            DisableATeleportPoint(activeIndex);

            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null) cc.enabled = false;
            player.transform.position = teleportPosition;
            if (cc != null) cc.enabled = true;

            teleportPosition = Vector3.zero;          
        }

        #endregion

        #region Enable/Disable TeleportPoints

        private void EnableAllPoints()
        {
            foreach (var point in teleportingPoints)
            {
                point.gameObject.SetActive(true);
            }
        }

        public void DisableATeleportPoint(int index)
        {
            if (index < 0 || index > teleportingPoints.Count - 1) return;          
            teleportingPoints[index].gameObject.SetActive(false);
        }

        public void DisableATeleportPoint(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            foreach (var point in teleportingPoints)
            {
                if (point.name == name)
                {
                    point.gameObject.SetActive(false);
                    return;
                }
            }
        }

        public void EnableATeleportPoint(int index)
        {
            if (index < 0 || index > teleportingPoints.Count - 1) return;
            teleportingPoints[index].gameObject.SetActive(true);
        }

        public void EnableATeleportPoint(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            foreach (var point in teleportingPoints)
            {
                if (point.name == name)
                {
                    point.gameObject.SetActive(true);
                    return;
                }
            }
        }

        #endregion
    }
}


