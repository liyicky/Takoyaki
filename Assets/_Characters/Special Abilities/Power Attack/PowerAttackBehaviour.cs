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

        public override void Use(AbilityUseParams useParams)
        {
            float adjDamage = useParams.baseDamage + (ability as PowerAttack).Damage();
            useParams.target.TakeDamage(adjDamage);
            PlayAbilitySound();
            PlayParticleEffect();
        }
    }
}
