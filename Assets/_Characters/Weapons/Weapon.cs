﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
  [CreateAssetMenu(menuName=("RPG/Weapon"))]

  public class Weapon : ScriptableObject
  {
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] AnimationClip attackAnimation;
    [SerializeField] float attackRadius = 1f;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] float weaponDamage = 10f;
    [SerializeField] float damageDelay = 0.5f;

    public Transform girpTransform;

    public GameObject GetWeaponPrefab()
    {
      return weaponPrefab;
    }

    public AnimationClip GetAttackAnimClip()
    {
      // RemoveAnimationEvents();
      return attackAnimation;
    }

    public float AttackRadius()
    {
      return attackRadius;
    }

    public float AttackCooldown()
    {
      return attackCooldown;
    }

    public float WeaponDamage()
    {
      return weaponDamage;
    }

    public float GetDamageDelay()
    {
      return damageDelay;
    }

    // Stops bad stuff from happening when there are events
    private void RemoveAnimationEvents()
    {
      attackAnimation.events = new AnimationEvent[0];
    }
  }
}
