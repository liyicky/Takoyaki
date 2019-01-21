﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Weapon;
using RPG.Core;
using System;

namespace RPG.Characters
{
  [RequireComponent(typeof(CameraRaycaster))]

  public class Player : MonoBehaviour, IDamageable
  {
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float maxManaPoints = 100f;
    [SerializeField] float manaRegenRate = 1f;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] RPG.Weapon.Weapon weaponInUse;
    [SerializeField] AnimatorOverrideController animatorOverrideController;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }
    public float manaAsPercentage { get { return currentManaPoints / maxManaPoints; } }

    CameraRaycaster cameraRaycaster;
    GameObject currentTarget;
    float currentHealthPoints;
    float currentManaPoints;
    float lastHitTime = 1f;

    public void TakeDamage(float damage)
    {
      // Mathf.Clamp - Only allows values between two floats (i.e. 0 and maxHealth)
      currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
    }

    // Start is called before the first frame update
    private void Start()
    {
      SetCurrentPoints();
      SetupMouseClick();
      PutWeaponInHand();
      OverrideAnimatorController();
    }

    private void SetCurrentPoints()
    {
      currentHealthPoints = maxHealthPoints;
      currentManaPoints = maxManaPoints;
    }

    private void OverrideAnimatorController()
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
      if (Input.GetMouseButtonDown(1))
      {
        currentManaPoints -= 10;
        currentTarget = enemy.gameObject;
        Attack();
      }
    }

    private void PutWeaponInHand()
    {
      var weaponPrefab = weaponInUse.GetWeaponPrefab();
      GameObject dominantHand = RequestDominantHand();
      GameObject weapon = Instantiate(weaponPrefab, dominantHand.transform) as GameObject;
      weapon.transform.localPosition = weaponInUse.girpTransform.localPosition;
      weapon.transform.localRotation = weaponInUse.girpTransform.localRotation;
    }

    private GameObject RequestDominantHand()
    {
      var dominantHands = GetComponentsInChildren<DominantHand>();
      int numberOfDominantHands = dominantHands.Length;
      Assert.AreNotEqual(numberOfDominantHands, 0, "No Dominant Hand");
      Assert.IsFalse(numberOfDominantHands > 1, "Multiple domainant hand scripts on player");
      return dominantHands[0].gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
      RegenMana();
    }
    
    private void RegenMana()
    {
      float calculatedManaRegen = (0.01f * manaRegenRate) * currentManaPoints;
      currentManaPoints = Mathf.Clamp(calculatedManaRegen + currentManaPoints, 0f, maxManaPoints);
    }

    private void Attack()
    {            
      var targetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);
      if (targetDistance > weaponInUse.AttackRadius()) return;
      if (Time.time - lastHitTime > weaponInUse.AttackCooldown())
      {
        (currentTarget.GetComponent(typeof(IDamageable)) as IDamageable).TakeDamage(attackDamage);
        lastHitTime = Time.time;
        AttackAnimation();
      }
    }

    private void AttackAnimation()
    {
      var animator = GetComponent<Animator>();
      animator.SetTrigger("Attack");
    }
  }
}