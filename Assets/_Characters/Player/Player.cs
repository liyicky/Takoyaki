using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Core;
using System;

namespace RPG.Characters
{
  [RequireComponent(typeof(CameraRaycaster))]

  public class Player : MonoBehaviour
  {
    [SerializeField] float baseDamage = 10f;
    [SerializeField] Weapon weaponInUse;
    [SerializeField] AnimatorOverrideController animatorOverrideController;
    [Range(.01f, 1.0f)] [SerializeField] float criticalHitChance = .01f;
    [SerializeField] float criticalHitMultiplayer = 2f;
    [SerializeField] ParticleSystem critParticleSystem;

    const string ATTACK_TRIGGER = "Attack";

    CameraRaycaster cameraRaycaster;
    GameObject currentTarget;
    GameObject currentWeapon;
    SpecialAbilities abilities;
    
    
    float lastHitTime = 1f;

    Animator animator;

    public void PutWeaponInHand(Weapon weaponConfig)
    {
      weaponInUse = weaponConfig;
      GameObject dominantHand = RequestDominantHand();
      Destroy(currentWeapon);
      currentWeapon = Instantiate(weaponInUse.GetWeaponPrefab(), dominantHand.transform) as GameObject;
      currentWeapon.transform.localPosition = weaponInUse.girpTransform.localPosition;
      currentWeapon.transform.localRotation = weaponInUse.girpTransform.localRotation;
    }

    // Start is called before the first frame update
    private void Start()
    {
      SetupMouseClick();
      PutWeaponInHand(weaponInUse);
      SetAttackAnimation();
      abilities = GetComponent<SpecialAbilities>();
      animator = GetComponent<Animator>();
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

    private void SetAttackAnimation()
    {
      var animator = GetComponent<Animator>();
      animator.runtimeAnimatorController = animatorOverrideController;
      animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // TODO: remove const
    }

    private void SetupMouseClick()
    {
      cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      cameraRaycaster.onMouseOverEnemy += ProcessEnemyInteraction;
    }

    private void ProcessEnemyInteraction(Enemy enemy)
    {
      currentTarget = enemy.gameObject;
      if (Input.GetMouseButtonDown(0))
      {
        Attack();
      }
      else if (Input.GetMouseButtonDown(1))
      {
        abilities.AttemptSpecialAbility(2);
      }
    }

    private GameObject RequestDominantHand()
    {
      var dominantHands = GetComponentsInChildren<DominantHand>();
      int numberOfDominantHands = dominantHands.Length;
      Assert.AreNotEqual(numberOfDominantHands, 0, "No Dominant Hand");
      Assert.IsFalse(numberOfDominantHands > 1, "Multiple domainant hand scripts on player");
      return dominantHands[0].gameObject;
    }
    


    private float CalculateDamage()
    {
      float damageBeforeCrit = baseDamage + weaponInUse.WeaponDamage();
      bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
      if (isCriticalHit)
      {
        critParticleSystem.Play();
        return damageBeforeCrit * criticalHitMultiplayer;
      }
      return damageBeforeCrit;
    }

    private void Attack()
    {            
      var targetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);
      if (targetDistance > weaponInUse.AttackRadius()) return;
      if (Time.time - lastHitTime > weaponInUse.AttackCooldown())
      {
        currentTarget.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
        lastHitTime = Time.time;
        AttackAnimation();
      }
    }

    void ScanForAbilityKeyDown()
    {
      if (Input.GetKeyDown("1"))
      {
        abilities.AttemptSpecialAbility(0);
      }
      else if (Input.GetKeyDown("2"))
      {
        currentTarget = gameObject;
        abilities.AttemptSpecialAbility(1);
      }
    }

    private void AttackAnimation()
    {
      SetAttackAnimation();
      animator.SetTrigger(ATTACK_TRIGGER);
    }
  }
}