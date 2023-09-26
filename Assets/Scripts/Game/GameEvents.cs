using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR.InteractionSystem;

public class GameEvents : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private static Vector3 teleportPosition = Vector3.zero;
    private Quaternion teleportPosRotation;

    public static void SetTeleportPosition(Vector3 position)
    {
        teleportPosition = position;
    }

    public void SetTeleportpoint(GameObject obj)
    {
        if(obj == null) return; 
        teleportPosition = obj.transform.position;
        teleportPosRotation = obj.transform.rotation;
    }

    public void TeleportPlayer()
    {
        if (teleportPosition == Vector3.zero) return;

        CharacterController cc = player.GetComponent<CharacterController>();
              
        if (cc != null)  cc.enabled = false;
        player.transform.position = teleportPosition;
        player.transform.rotation = teleportPosRotation;
        if (cc != null) cc.enabled = true;
        
        teleportPosition = Vector3.zero;
    }

    public void ChangeHandState(bool active)
    {
        foreach (var hand in Player.instance.hands)
        {
            hand.enabled = active;
        }
    }
}
