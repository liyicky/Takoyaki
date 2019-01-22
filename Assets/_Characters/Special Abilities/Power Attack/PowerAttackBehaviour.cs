using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{    
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttack powerAttack;

        public float ManaCost()
        {
            return ManaCost();
        }

        public void SetConfig(PowerAttack powerAttackToSet)
        {
            this.powerAttack = powerAttackToSet;
        }

        public void Use(AbilityUseParams useParams)
        {
            float adjDamage = useParams.baseDamage + powerAttack.Damage();
            useParams.target.TakeDamage(adjDamage);
            PlayParticleEffect();
        }

        void PlayParticleEffect()
        {
            GameObject prefab = Instantiate(powerAttack.GetParticalPrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}
