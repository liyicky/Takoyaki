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
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
