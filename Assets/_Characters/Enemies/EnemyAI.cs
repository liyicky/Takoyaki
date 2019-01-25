using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{

  [RequireComponent(typeof(WeaponSystem))]
  public class EnemyAI : MonoBehaviour
  {
    [SerializeField] float aggroRadius = 10f;

    PlayerControl player;
    float currentWeaponRange;
    bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
      player = FindObjectOfType<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
      WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
      currentWeaponRange = weaponSystem.GetCurrentWeapon().AttackRadius();


      float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

      if (distanceToPlayer <= currentWeaponRange && !isAttacking)
      {
        isAttacking = true;
        // float randomizedDelay = fireInterval * Random.Range(fireInterval - fireVariation, fireInterval + fireVariation);
        // InvokeRepeating("FireProjectile", 0f, randomizedDelay); //TODO: Switch to coroutines
      }

      if (distanceToPlayer > currentWeaponRange)
      {
        isAttacking = false;
        CancelInvoke("FireProjectile");
      }

      if (distanceToPlayer <= aggroRadius)
      {   
        // aiController.SetTarget(player.transform);
      }
      else
      {
        // aiController.SetTarget(transform);
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