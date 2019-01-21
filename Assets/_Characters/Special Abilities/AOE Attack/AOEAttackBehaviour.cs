using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

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
            // var ray = new Ra
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

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
