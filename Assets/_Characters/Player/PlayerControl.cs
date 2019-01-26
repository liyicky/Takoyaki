using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;
using RPG.Core;
using System;

namespace RPG.Characters
{
  public class PlayerControl : MonoBehaviour
  {
    Character character;
    GameObject currentTarget;
    SpecialAbilities abilities;
    Animator animator;
    WeaponSystem weaponSystem;

    // Start is called before the first frame update
    private void Start()
    {
      RegisterForMouseEvents();
      // PutWeaponInHand(weaponInUse);

      abilities = GetComponent<SpecialAbilities>();
      animator = GetComponent<Animator>();
      character = GetComponent<Character>();
      weaponSystem = GetComponent<WeaponSystem>();
    }
    
    // Update is called once per frame
    private void Update()
    {
      var healthPercentage = GetComponent<HealthSystem>().healthAsPercentage;
      if (healthPercentage > Mathf.Epsilon)
      {
        ScanForAbilityKeyDown();
      }
    }

    public GameObject GetCurrentTarget()
    {
      return currentTarget;
    }

    private void RegisterForMouseEvents()
    {
      CameraRaycaster cameraRaycaster;
      cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      cameraRaycaster.onMouseOverEnemy += ProcessEnemyInteraction;
      cameraRaycaster.onMouseOverTerrain += ProcessTerrainInteraction;
    }

    private void ProcessEnemyInteraction(EnemyAI enemy)
    {
      if (Input.GetMouseButtonDown(0) && IsTargetInRange(enemy.gameObject))
      {
        currentTarget = enemy.gameObject;
        weaponSystem.Attack(enemy.gameObject);
      }
      else if (Input.GetMouseButtonDown(0) && !IsTargetInRange(enemy.gameObject))
      {
        StartCoroutine(MoveAndAttack(enemy));
      }
      else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.gameObject))
      {
        abilities.AttemptSpecialAbility(2, currentTarget);
      }
      else if (Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.gameObject))
      {
        StartCoroutine(MoveAndPowerAttack(enemy));
      }
    }

    IEnumerator MoveAndTarget(EnemyAI target)
    {
      character.SetDestination(target.transform.position);
      while (!IsTargetInRange(target.gameObject))
      {
        yield return new WaitForEndOfFrame();
      }
      yield return new WaitForEndOfFrame();
    }

    IEnumerator MoveAndAttack(EnemyAI target)
    {
      yield return StartCoroutine(MoveAndTarget(target));
      weaponSystem.Attack(target.gameObject);
    }

    IEnumerator MoveAndPowerAttack(EnemyAI target)
    {
      yield return StartCoroutine(MoveAndTarget(target));
      abilities.AttemptSpecialAbility(2, target.gameObject);
    }
    

    private void ProcessTerrainInteraction(Vector3 destination)
    {
      if (Input.GetMouseButton(0))
      {
        weaponSystem.StopAttacking();
        character.SetDestination(destination);
      }
    }

    private bool IsTargetInRange(GameObject target)
    {
      float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
      return distanceToTarget <= weaponSystem.GetCurrentWeapon().AttackRadius();
    }

    void ScanForAbilityKeyDown()
    {
      if (!currentTarget) currentTarget = gameObject;
      if (Input.GetKeyDown("1"))
      {
        abilities.AttemptSpecialAbility(0, currentTarget);
      }
      else if (Input.GetKeyDown("2"))
      {
        abilities.AttemptSpecialAbility(1, gameObject);
      }
    }
  }
}