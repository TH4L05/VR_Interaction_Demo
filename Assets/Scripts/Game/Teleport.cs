using System.Collections.Generic;
using UnityEngine;

namespace eecon_lab
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private List<GameObject> teleportPoints = new List<GameObject>();

        private GameObject player;
        private Vector3 teleportPosition = Vector3.zero;
        private Quaternion teleportPosRotation;
        private int lastIndex = -1;

        private void Start()
        {
            player = Game.Instance.PlayerGO;
        }

        public void SetTelportPosition(Vector3 teleportPosition)
        {
            if (teleportPosition == Vector3.zero) return;
            this.teleportPosition = teleportPosition;
        }

        public void SetTeleportPosition(int index)
        {

        }

        public void SetTeleport(GameObject teleportPoint)
        {
            //Debug.Log("SetTeleport");
            if(teleportPoint == null) return;
            int index = 0;

            foreach (var point in teleportPoints)
            {
                if (point.name == teleportPoint.name)
                {
                    //var coll = point.GetComponentInChildren<BoxCollider>();
                    //float offset = coll.center.y;

                    teleportPosition = point.transform.position;
                    //teleportPosition.y -= offset;
                    lastIndex = index;
                    return;
                }
                index++;
            }
        }

        public void SetTeleportByGameObject(GameObject obj)
        {
            if (obj == null) return;
            teleportPosition = obj.transform.position;
            teleportPosRotation = obj.transform.rotation;
        }

        public void TeleportPlayer()
        {
            if (teleportPosition == Vector3.zero) return;

            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null) cc.enabled = false;
            player.transform.position = teleportPosition;
            player.transform.rotation = teleportPosRotation;
            if (cc != null) cc.enabled = true;

            teleportPosition = Vector3.zero;
            EnableAllPoints();
            DisableATeleportPoint(lastIndex);
        }

        private void EnableAllPoints()
        {
            foreach (var point in teleportPoints)
            {
                point.SetActive(true);
            }
        }

        public void DisableATeleportPoint(int index)
        {
            teleportPoints[index].SetActive(false);
        }

        public void DisableATeleportPoint(string name)
        {
            foreach (var point in teleportPoints)
            {
                if (point.name == name)
                {
                    point.SetActive(false);
                    return;
                }
            }
        }

        public void EnableATeleportPoint(int index)
        {
            teleportPoints[index].SetActive(true);
        }

        public void EnableATeleportPoint(string name)
        {
            foreach (var point in teleportPoints)
            {
                if (point.name == name)
                {
                    point.SetActive(true);
                    return;
                }
            }
        }
    }
}


