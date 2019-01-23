using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
  {
  [RequireComponent(typeof (NavMeshAgent))]
  [RequireComponent(typeof (ThirdPersonCharacter))]
  [RequireComponent(typeof(CameraRaycaster))]

  public class CharacterMovement : MonoBehaviour
  {
    [SerializeField] float stoppingDistance = 1f;
    ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
    GameObject walkTarget;
    NavMeshAgent agent;
    // bool isInDirectMode = false;
        
    private void Start()
    {
      CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      character = GetComponent<ThirdPersonCharacter>();

      agent = GetComponent<NavMeshAgent>();
      agent.updateRotation = false;
      agent.updatePosition = true;
      agent.stoppingDistance = stoppingDistance;

      walkTarget = new GameObject("walkTarget");

      cameraRaycaster.onMouseOverTerrain += ProcessMouseMovement;
      cameraRaycaster.onMouseOverEnemy += ProcessEnemyInteraction;
    }

    private void Update() {
      if (agent.remainingDistance > agent.stoppingDistance)
      {
        character.Move(agent.desiredVelocity);
      }
      else
      {
        character.Move(Vector3.zero);
      }
    }

    private void ProcessEnemyInteraction(Enemy enemy)
    {
      if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
      {
        agent.SetDestination(enemy.transform.position);
      }
    }

    private void ProcessMouseMovement(Vector3 destination)
    {
      if (Input.GetMouseButton(0))
      {
        agent.SetDestination(destination);
      }
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
