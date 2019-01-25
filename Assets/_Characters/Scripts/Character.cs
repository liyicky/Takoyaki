using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
  {
  [SelectionBase]
  public class Character : MonoBehaviour
  {
    [Header("Setup Settings")]
    [SerializeField] RuntimeAnimatorController animatorController;
    [SerializeField] AnimatorOverrideController animatorOverrideController;
    [SerializeField] Avatar characterAvatar;

    [Header("Capsule Settings")]
    [SerializeField] Vector3 capsuleCenter = new Vector3(0f, 0.8f, 0f);
    [SerializeField] float capsuleRadius = 0.2f;
    [SerializeField] float capsuleHeight = 1.6f;

    [Header("NavMeshAgent Steering Settings")]
    [SerializeField] float agentSpeed = 1f;
    [SerializeField] float agentAngularSpeed = 120f;
    [SerializeField] float agentAcceleration = 8f;
    [SerializeField] float agentStoppingDistance = 3f;

    [Header("NavMeshAgent Obstical Avoidance Settings")]
    [SerializeField] float agentOVRadius = 0.1f;

    [Header("MeshFilter Settings")]
    [SerializeField] Mesh characterMesh;

    [Header("Movement Settings")]
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
    bool isAlive = true;
        
    // Callback
		public void OnAnimatorMove() {
			if (Time.deltaTime > 0)
			{
				Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
			}
		}

    public AnimatorOverrideController GetOverrideController()
    {
      return animatorOverrideController;
    }

    public void Kill()
    {
      isAlive = false;
    }

    public bool StillAlive()
    {
      return GetComponent<HealthSystem>().healthAsPercentage > Mathf.Epsilon;
    }

    public void SetDestination(Vector3 destination)
    {
      agent.SetDestination(destination);
    }

    public float GetAnimSpeedMultiplier()
    {
      return animator.speed;
    }
    
    private void Awake() {
        AddRequiredComponents();
    }

    void AddRequiredComponents()
    {
      animator = gameObject.AddComponent<Animator>();
      animator.runtimeAnimatorController = animatorController;
      animator.avatar = characterAvatar;

      var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
      capsuleCollider.center = capsuleCenter;
      capsuleCollider.radius = capsuleRadius;
      capsuleCollider.height = capsuleHeight;

      rigidbody = gameObject.AddComponent<Rigidbody>();

      agent = gameObject.AddComponent<NavMeshAgent>();
      agent.updateRotation = false;
      agent.updatePosition = true;
      agent.stoppingDistance = stoppingDistance;
      agent.speed = agentSpeed;
      agent.angularSpeed = agentAngularSpeed;
      agent.acceleration = agentAcceleration;
      agent.stoppingDistance = agentStoppingDistance;
      agent.radius = agentOVRadius;

      var mashRenderer = gameObject.AddComponent<MeshRenderer>();

      var meshFilter = gameObject.AddComponent<MeshFilter>();
      meshFilter.mesh = characterMesh;

      var audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
      rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update() {
      if (agent.remainingDistance > agent.stoppingDistance && isAlive)
      {
        Move(agent.desiredVelocity);
      }
      else
      {
        Move(Vector3.zero);
      }
    }

    private void Move(Vector3 move)
    {
      SetForwardAndTurn(move);
      ApplyExtraTurnRotation();
      UpdateAnimator();
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
