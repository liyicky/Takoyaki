﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{    
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse;
        [Range(.01f, 1.0f)] [SerializeField] float criticalHitChance = .01f;
        [SerializeField] float criticalHitMultiplayer = 2f;
        [SerializeField] ParticleSystem critParticleSystem;

        const string ATTACK_TRIGGER = "Attack";

        Animator animator;
        GameObject currentWeapon;
        float lastHitTime = 1f;
        GameObject currentTarget;
        Character character;


        // Start is called before the first frame update
        void Start()
        {
            character = GetComponent<Character>();
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = character.GetOverrideController();
            SetAttackAnimation();
            PutWeaponInHand(weaponInUse);
        }

        // Update is called once per frame
        void Update()
        {
            var player = GetComponent<PlayerControl>();
            if (player)
            {
                currentTarget = player.GetCurrentTarget();
            }
            else
            {
                currentTarget = gameObject;
            }
        }

        public Weapon GetCurrentWeapon()
        {
            return weaponInUse;
        }

        public void PutWeaponInHand(Weapon weaponConfig)
        {
            weaponInUse = weaponConfig;
            GameObject dominantHand = RequestDominantHand();
            Destroy(currentWeapon);
            currentWeapon = Instantiate(weaponInUse.GetWeaponPrefab(), dominantHand.transform) as GameObject;
            currentWeapon.transform.localPosition = weaponInUse.girpTransform.localPosition;
            currentWeapon.transform.localRotation = weaponInUse.girpTransform.localRotation;
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

        public void Attack()
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

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No Dominant Hand");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple domainant hand scripts on player");
            return dominantHands[0].gameObject;
        }
            
        private void SetAttackAnimation()
        {
            character.GetOverrideController()["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // TODO: remove const
        }

        private void AttackAnimation()
        {
            SetAttackAnimation();
            animator.SetTrigger(ATTACK_TRIGGER);
        }
    }
}