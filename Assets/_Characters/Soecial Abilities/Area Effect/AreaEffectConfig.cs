using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area of Effect Attack"))]
    public class AreaEffectConfig : SpecialAbility
    {
        [Header("Area Effect Specific")]
        [SerializeField]
        float damageToEachTarget = 15f;

        [SerializeField]
        float radius = 5f;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviorComponent = gameObjectToAttachTo.AddComponent<AreaEffectBehavior>();
            behaviorComponent.SetConfig(this);

            base.behavior = behaviorComponent;
        }

        public float GetDamageToEachTarget()
        {
            return damageToEachTarget;
        }

        public float GetRadius()
        {
            return radius;
        }
    }
}