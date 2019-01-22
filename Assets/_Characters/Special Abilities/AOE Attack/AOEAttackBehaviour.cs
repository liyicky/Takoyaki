using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AOEAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        AOEAttack aoeAttack;

        public float ManaCost()
        {
            return ManaCost();
        }

        public void SetConfig(AOEAttack aoeAttackToSet)
        {
            this.aoeAttack = aoeAttackToSet;
        }

        public void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayParticalEffect();
        }

        private void PlayParticalEffect()
        {
            GameObject prefab = Instantiate(aoeAttack.GetParticalPrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }

        private void DealRadialDamage(AbilityUseParams useParams)
        {
            RaycastHit[] hitsInRange = Physics.SphereCastAll(useParams.location, aoeAttack.Radius(), Vector3.forward);
            foreach (var target in hitsInRange)
            {
                var damageable = target.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float adjDamage = aoeAttack.Damage() + useParams.baseDamage;
                    damageable.TakeDamage(adjDamage);
                }
            }
        }
    }
}
