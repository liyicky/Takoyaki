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
    CameraRaycaster cameraRaycaster;
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
      cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      cameraRaycaster.onMouseOverEnemy += ProcessEnemyInteraction;
      cameraRaycaster.onMouseOverTerrain += ProcessTerrainInteraction;
    }

    private void ProcessEnemyInteraction(EnemyAI enemy)
    {
      currentTarget = enemy.gameObject;
      if (Input.GetMouseButtonDown(0))
      {
        weaponSystem.Attack(enemy.gameObject);
      }
      else if (Input.GetMouseButtonDown(1))
      {
        abilities.AttemptSpecialAbility(2, currentTarget);
      }
    }

    private void ProcessTerrainInteraction(Vector3 destination)
    {
      if (Input.GetMouseButton(0))
      {
        character.SetDestination(destination);
      }
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