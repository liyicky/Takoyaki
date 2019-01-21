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

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public float Damage()
        {
            return damDam;
        }
    }   
}
