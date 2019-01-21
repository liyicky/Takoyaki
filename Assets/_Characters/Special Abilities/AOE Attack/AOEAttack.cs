using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName=("RPG/Special Ability/AOE Attack"))]

    public class AOEAttack : SpecialAbility
    {
        [Header("AOE Attack Specific")]
        [SerializeField] float radius;
        [SerializeField] float damage;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<AOEAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public float Radius()
        {
            return radius;
        }

        public float Damage()
        {
            return damage;
        }
    }
}
