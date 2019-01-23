using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
  {
  [RequireComponent(typeof (NavMeshAgent))]
  [RequireComponent(typeof(CameraRaycaster))]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Animator))]

  public class CharacterMovement : MonoBehaviour
  {
    [SerializeField] float stoppingDistance = 1f;
    [SerializeField] float moveSpeedMultiplier = 1f;
    [SerializeField] float animSpeedMultiplier = 1f;
    [SerializeField] float moveThreshold = 1f;

    NavMeshAgent agent;
    Animator animator;
    Rigidbody rigidbody;
    float movingTurnSpeed = 360;
		float stationaryTurnSpeed = 360;
		float turnAmount;
		float forwardAmount;
    // bool isInDirectMode = false;
        
    // Callback
		public void OnAnimatorMove() {
			if (Time.deltaTime > 0)
			{
				Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
			}
		}

    public void Move(Vector3 move)
    {
      SetForwardAndTurn(move);
      ApplyExtraTurnRotation();
      UpdateAnimator();
    }

    private void Start()
    {
      CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      animator = GetComponent<Animator>();
      rigidbody = GetComponent<Rigidbody>();
      agent = GetComponent<NavMeshAgent>();
      agent.updateRotation = false;
      agent.updatePosition = true;
      agent.stoppingDistance = stoppingDistance;

      cameraRaycaster.onMouseOverTerrain += ProcessMouseMovement;
      cameraRaycaster.onMouseOverEnemy += ProcessEnemyInteraction;

      rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update() {
      if (agent.remainingDistance > agent.stoppingDistance)
      {
        Move(agent.desiredVelocity);
      }
      else
      {
        Move(Vector3.zero);
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

    private void SetForwardAndTurn(Vector3 move)
    {
      if (move.magnitude > moveThreshold) move.Normalize();
      var localMove = transform.InverseTransformDirection(move);
      turnAmount = Mathf.Atan2(localMove.x, localMove.z);
      forwardAmount = localMove.z;
    }

    void UpdateAnimator()
		{
			// update the animator parameters
			animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
			animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
      animator.speed = animSpeedMultiplier;
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
			transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
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
