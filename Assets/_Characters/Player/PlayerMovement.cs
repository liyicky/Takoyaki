using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
  {
  [RequireComponent(typeof (NavMeshAgent))]
  [RequireComponent(typeof (AICharacterControl))]
  [RequireComponent(typeof (ThirdPersonCharacter))]
  [RequireComponent(typeof(CameraRaycaster))]

  public class PlayerMovement : MonoBehaviour
  {
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    AICharacterControl aiController = null;
    GameObject walkTarget = null;
    // bool isInDirectMode = false;
        
    private void Start()
    {
      cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
      aiController = GetComponent<AICharacterControl>();
      walkTarget = new GameObject("walkTarget");

      cameraRaycaster.onMouseOverTerrain += ProcessMouseMovement;
      cameraRaycaster.onMouseOverEnemy += ProcessEnemyInteraction;
    }

    private void ProcessEnemyInteraction(Enemy enemy)
    {
      if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
      {
        aiController.SetTarget(enemy.transform);
      }
    }

    private void ProcessMouseMovement(Vector3 destination)
    {
      if (Input.GetMouseButton(0))
      {
        walkTarget.transform.position = destination;
        aiController.SetTarget(walkTarget.transform);
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
}
