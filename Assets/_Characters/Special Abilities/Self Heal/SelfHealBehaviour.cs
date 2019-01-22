using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {
        SelfHeal selfHeal;

        public float ManaCost()
        {
            return ManaCost();
        }

        public void SetConfig(SelfHeal selfHealToSet)
        {
            this.selfHeal = selfHealToSet;
        }

        public void Use(AbilityUseParams useParams)
        {
            useParams.target.TakeDamage(selfHeal.Heal());
            PlayParticalEffect();
        }

        private void PlayParticalEffect()
        {
            GameObject prefab = Instantiate(selfHeal.GetParticalPrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}
