using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
[RequireComponent(typeof(CameraRaycaster))]

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] const int walkableLayerNumber = 9;
    [SerializeField] const int enemyLayerNumber = 10;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    bool isInDirectMode = false;
    AICharacterControl aiController = null;
    GameObject walkTarget = null;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        aiController = GetComponent<AICharacterControl>();
        cameraRaycaster.notifyMouseClickObservers += ProcessMouseMovement;
        walkTarget = new GameObject("walkTarget");
    }

    private void ProcessMouseMovement(RaycastHit raycastHit, int layerHit)
    {
        switch (layerHit)
        {
            case walkableLayerNumber:
                walkTarget.transform.position = raycastHit.point;
                aiController.SetTarget(walkTarget.transform);
                break;
            case enemyLayerNumber:
                aiController.SetTarget(raycastHit.collider.gameObject.transform);
                break;
            default:
                print("Don't know how to handle player movement here");
                return;
        }
    }

    private void ProcessKeyboardMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        thirdPersonCharacter.Move(v*camForward + h*Camera.main.transform.right, false, false);
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.black;
    //     Gizmos.DrawLine(transform.position, walkTarget.transform.position);
    //     Gizmos.DrawSphere(walkTarget.transform.position, 0.1f);
    //     Gizmos.DrawSphere(walkTarget.transform.position, 0.2f);

    //     Gizmos.color = new Color(255f, 0f, 0, 0.5f);
    // }
}

