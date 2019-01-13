using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
[RequireComponent(typeof(CameraRaycaster))]

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5f;
    [SerializeField] const int walkableLayerNumber = 9;
    [SerializeField] const int enemyLayerNumber = 10;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestinationPoint, clickPoint;
    bool isInDirectMode = false;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestinationPoint = transform.position;
        cameraRaycaster.notifyMouseClickObservers += ProcessMouseMovement;
    }

    private void ProcessMouseMovement(RaycastHit raycastHit, int layerHit)
    {

        clickPoint = raycastHit.transform.position;
        switch (layerHit)
        {
            case walkableLayerNumber:
                currentDestinationPoint = ShortDestination(clickPoint, walkMoveStopRadius);
                break;
            case enemyLayerNumber:
                currentDestinationPoint = ShortDestination(clickPoint, attackMoveStopRadius);
                break;
            default:
                print("ERROR");
                return;
            }

        WalkToDestination();
    }

    void WalkToDestination()
    {
        var playerToClickPoint = currentDestinationPoint - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    private void ProcessKeyboardMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        thirdPersonCharacter.Move(v*camForward + h*Camera.main.transform.right, false, false);
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestinationPoint);
        Gizmos.DrawSphere(currentDestinationPoint, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.2f);

        Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}

