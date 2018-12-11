using System;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster = null;
    AICharacterControl aiCharacterControl = null;
    Vector3 currentDestination = Vector3.zero, clickPoint = Vector3.zero;

    GameObject walkTarget = null;

    bool isInDirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        aiCharacterControl = GetComponent<AICharacterControl>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        walkTarget = new GameObject("walkTarget");

        cameraRaycaster.notifyMouseClickObservers += ProcesdsMouseClick;

    }

    private void ProcesdsMouseClick(RaycastHit hit, int layer)
    {
        var walkTo = transform;
        Debug.Log(layer);
        switch(layer)
        {
            case 8: //walkable
                walkTarget.transform.position = hit.point;
                walkTo = walkTarget.transform;
                break;
            case 9: //enemy
                walkTo = hit.collider.gameObject.transform;
                break;
            default:
                break;
        }

        aiCharacterControl.SetTarget(walkTo);
    }

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, false, false);
    }
}

