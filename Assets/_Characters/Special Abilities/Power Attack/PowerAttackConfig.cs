using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName=("RPG/Speical Ability/Power Attack"))]

    public class PowerAttackConfig : SpecialAbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float damDam;

        public override ISpecialAbility AddComponent(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            return behaviourComponent;
        }
    }
}
