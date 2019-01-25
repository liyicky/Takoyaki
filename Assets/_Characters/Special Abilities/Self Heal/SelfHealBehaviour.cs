using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        public float ManaCost()
        {
            return ManaCost();
        }

        public override void Use(GameObject target)
        {
            target.GetComponent<HealthSystem>().TakeDamage((ability as SelfHeal).Heal());
            PlayAbilitySound();
            PlayAnimation();
            PlayParticleEffect();
        }
    }
}
