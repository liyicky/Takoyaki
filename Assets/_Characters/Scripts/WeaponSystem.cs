using System.Collections;
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

        void Start()
        {
            character = GetComponent<Character>();
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = character.GetOverrideController();
            SetAttackAnimation();
            PutWeaponInHand(weaponInUse);
        }

        void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;
            bool characterIsDead = !character.StillAlive();

            if (currentTarget == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {   
                var targetDistance = Vector3.Distance(currentTarget.transform.position, transform.position);
                targetIsDead = !currentTarget.GetComponent<Character>().StillAlive();
                targetIsOutOfRange = targetDistance > weaponInUse.AttackRadius();
            }

            if (characterIsDead || targetIsDead || targetIsOutOfRange)
            {
                StopAllCoroutines();
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

        public void StopAttacking()
        {
            animator.StopPlayback();
            StopAllCoroutines();
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

        public void Attack(GameObject target)
        {            
            currentTarget = target;
            // StopAllCoroutines();
            StartCoroutine(AttackTargetRepeatedly());
        }

        IEnumerator DamageAfterDelay(float damageDelay)
        {
            yield return new WaitForSecondsRealtime(damageDelay);
            currentTarget.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());            
        }

        IEnumerator AttackTargetRepeatedly()
        {
            
            while (character.StillAlive() && currentTarget.GetComponent<Character>().StillAlive())
            {
                var animClip = weaponInUse.GetAttackAnimClip();
                float animClipTime = animClip.length / character.GetAnimSpeedMultiplier();
                float timeToWait = animClipTime + weaponInUse.AttackCooldown();

                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;
                if (isTimeToHitAgain)
                {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        private void AttackTargetOnce()
        {
            transform.LookAt(currentTarget.transform);
            StartCoroutine(DamageAfterDelay(weaponInUse.GetDamageDelay()));
            AttackAnimation();
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
