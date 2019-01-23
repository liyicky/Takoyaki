using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AOEAttackBehaviour : AbilityBehaviour
    {
        public float ManaCost()
        {
            return ManaCost();
        }

        public override void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayParticleEffect();
        }

        private void DealRadialDamage(AbilityUseParams useParams)
        {
            RaycastHit[] hitsInRange = Physics.SphereCastAll(useParams.location, (ability as AOEAttack).Radius(), Vector3.forward);
            foreach (var target in hitsInRange)
            {
                var damageable = target.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float adjDamage = (ability as AOEAttack).Damage() + useParams.baseDamage;
                    damageable.TakeDamage(adjDamage);
                }
            }
        }
    }
}
