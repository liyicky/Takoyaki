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

        public override void Use(GameObject target)
        {
            DealRadialDamage(target);
            PlayAbilitySound();
            PlayAnimation();
            PlayParticleEffect();
        }

        private void DealRadialDamage(GameObject target)
        {
            RaycastHit[] hitsInRange = Physics.SphereCastAll(target.transform.position, (ability as AOEAttack).Radius(), Vector3.forward);
            foreach (var tar in hitsInRange)
            {
                var healthSys = tar.collider.GetComponent<HealthSystem>();
                if (healthSys) healthSys.TakeDamage((ability as AOEAttack).Damage());
            }
        }
    }
}
