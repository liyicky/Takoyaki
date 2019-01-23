using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName=("RPG/Special Ability/Power Attack"))]

    public class PowerAttack : SpecialAbility
    {
        [Header("Power Attack Specific")]
        [SerializeField] float damDam;

        protected override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<PowerAttackBehaviour>();
        }

        public float Damage()
        {
            return damDam;
        }
    }   
}
