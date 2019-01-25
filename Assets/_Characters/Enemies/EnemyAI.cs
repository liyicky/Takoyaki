using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{

  [RequireComponent(typeof(Character))]
  [RequireComponent(typeof(WeaponSystem))]
  public class EnemyAI : MonoBehaviour
  {
    [SerializeField] float aggroRadius = 10f;

    Character character;
    PlayerControl player;
    float currentWeaponRange;
    float distanceToPlayer;
    
    enum State { idle, attacking, patrolling, chasing }
    State state = State.idle;

    // Start is called before the first frame update
    void Start()
    {
      player = FindObjectOfType<PlayerControl>();
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
        //stop and start patrolling
        state = State.patrolling;
        StopAllCoroutines();
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
        // stop and attack the player
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

    void OnDrawGizmos()
    {
      Gizmos.color = new Color(255f, 0f, 0, 0.5f);
      Gizmos.DrawWireSphere(transform.position, aggroRadius);

      Gizmos.color = new Color(66f, 134f, 244f, 0.5f);
      Gizmos.DrawWireSphere(transform.position, currentWeaponRange);
    }
  }
}