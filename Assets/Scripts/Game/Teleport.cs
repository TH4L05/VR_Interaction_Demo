/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;

namespace eecon_lab
{
    public class Teleport : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] private bool isEnabled = true;
        [SerializeField] private List<Transform> teleportPoints = new List<Transform>();

        #endregion

        #region PrivateFields

        private GameObject player;
        private Vector3 teleportPosition = Vector3.zero;
        private Quaternion teleportPosRotation;
        private int activeIndex = -1;
        private int lastactiveIndex = -1;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            player = Game.Instance.PlayerGO;
        }

        #endregion

        #region SetTeleportPosition

        public void SetTelportPosition(Vector3 teleportPosition)
        {
            if (teleportPosition == Vector3.zero) return;
            this.teleportPosition = teleportPosition;
        }

        public void SetTeleportPosition(GameObject obj)
        {
            if (obj == null) return;
            teleportPosition = obj.transform.position;
            teleportPosRotation = obj.transform.rotation;
        }

        public void SetTeleport(int index)
        {
            teleportPosition = teleportPoints[index].position;
        }

        public void SetTeleport(GameObject teleportPointobject)
        {
            if (teleportPointobject == null) return;
            int index = 0;
            lastactiveIndex = activeIndex;

            foreach (var point in teleportPoints)
            {
                if (point.name == teleportPointobject.name)
                {
                    teleportPosition = point.transform.position;
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
            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null) cc.enabled = false;
            player.transform.position = teleportPosition;
            player.transform.rotation = teleportPosRotation;
            if (cc != null) cc.enabled = true;

            teleportPosition = Vector3.zero;
            EnableATeleportPoint(lastactiveIndex);
            DisableATeleportPoint(activeIndex);
        }

        #endregion

        #region Enable/Disable TeleportPoints

        private void EnableAllPoints()
        {
            foreach (var point in teleportPoints)
            {
                point.gameObject.SetActive(true);
            }
        }

        public void DisableATeleportPoint(int index)
        {
            if (index < 0 || index > teleportPoints.Count - 1) return;
            teleportPoints[index].gameObject.SetActive(false);
        }

        public void DisableATeleportPoint(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            foreach (var point in teleportPoints)
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
            if (index < 0 || index > teleportPoints.Count - 1) return;
            teleportPoints[index].gameObject.SetActive(true);
        }

        public void EnableATeleportPoint(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            foreach (var point in teleportPoints)
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


