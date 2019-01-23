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

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<SelfHealBehaviour>();
            behaviourComponent.SetAbility(this);
            behaviour = behaviourComponent;
        }

        public float Heal()
        {
            return -heal;
        }
    }   
}
