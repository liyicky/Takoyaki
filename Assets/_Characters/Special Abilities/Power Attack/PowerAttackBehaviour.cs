using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{    
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public float ManaCost()
        {
            return ManaCost();
        }

        public override void Use(GameObject target)
        {
            float adjDamage = (ability as PowerAttack).Damage();
            target.GetComponent<HealthSystem>().TakeDamage(adjDamage);
            PlayAbilitySound();
            PlayParticleEffect();
        }
    }
}
