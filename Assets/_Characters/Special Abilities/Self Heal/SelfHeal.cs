using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName=("RPG/Special Ability/Self Heal"))]

    public class SelfHeal : SpecialAbility
    {
        [Header("Power Attack Specific")]
        [SerializeField] float heal;

        protected override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<SelfHealBehaviour>();
        }

        public float Heal()
        {
            return -heal;
        }
    }   
}
