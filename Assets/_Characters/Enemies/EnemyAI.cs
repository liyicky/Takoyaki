using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{

  [RequireComponent(typeof(HealthSystem))]
  [RequireComponent(typeof(Character))]
  [RequireComponent(typeof(WeaponSystem))]
  public class EnemyAI : MonoBehaviour
  {
    [SerializeField] float aggroRadius = 10f;
    [SerializeField] float waypointTolerance = 0.2f;
    [SerializeField] WaypointContainer patrolPath;


    Character character;
    PlayerControl player;
    WeaponSystem weaponSystem;

    int nextWaypointIndex;
    float currentWeaponRange;
    float distanceToPlayer;
    
    enum State { idle, attacking, patrolling, chasing }
    State state = State.idle;

    // Start is called before the first frame update
    void Start()
    {
      player = FindObjectOfType<PlayerControl>();
      weaponSystem = GetComponent<WeaponSystem>();
    }

    // Update is called once per frame
    void Update()
    {
      character = GetComponent<Character>();
      WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
      currentWeaponRange = weaponSystem.GetCurrentWeapon().AttackRadius();
      distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

      if (distanceToPlayer > aggroRadius && state != State.patrolling)
      {
        StopAllCoroutines();
        StartCoroutine(Patrol());
      }
      if (distanceToPlayer <= aggroRadius && state != State.chasing)
      {
        StopAllCoroutines();
        StartCoroutine(ChasePlayer());
      }
      if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
      {
        state = State.attacking;
        StopAllCoroutines();
        StartCoroutine(MoveAndAttack(player.gameObject));
      }
    }

    IEnumerator Patrol()
    {
      state = State.patrolling;
      while (true)
      {
        Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).transform.position;
        character.SetDestination(nextWaypointPos);
        CycleWaypointWhenClose(nextWaypointPos);
        yield return new WaitForSecondsRealtime(0.5f);
      }
    }

    IEnumerator ChasePlayer()
    {
      state = State.chasing;
      while (distanceToPlayer >= currentWeaponRange)
      {
        character.SetDestination(player.transform.position);
        yield return new WaitForEndOfFrame();
      }
    }

    IEnumerator MoveAndTarget(GameObject target)
    {
      character.SetDestination(target.transform.position);
      while (!IsTargetInRange(target))
      {
        yield return new WaitForEndOfFrame();
      }
      yield return new WaitForEndOfFrame();
    }

    IEnumerator MoveAndAttack(GameObject target)
    {
      yield return StartCoroutine(MoveAndTarget(target));
      weaponSystem.Attack(target);
    }

    private void CycleWaypointWhenClose(Vector3 waypointPos)
    {
      if (Vector3.Distance(transform.position, waypointPos) <= waypointTolerance)
      {
        nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
      }
    }

    private bool IsTargetInRange(GameObject target)
    {
      float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
      return distanceToTarget <= weaponSystem.GetCurrentWeapon().AttackRadius();
    }

    void OnDrawGizmos()
    {
      Gizmos.color = new Color(255f, 0f, 0, 0.5f);
      Gizmos.DrawWireSphere(transform.position, aggroRadius);

      Gizmos.color = new Color(66f, 134f, 244f, 0.5f);
      Gizmos.DrawWireSphere(transform.position, currentWeaponRange);
    }
  }
}