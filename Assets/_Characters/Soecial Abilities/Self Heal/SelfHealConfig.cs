using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Self Heal"))]
    public class SelfHealConfig : SpecialAbility
    {
        [Header("Self Heal Specific")]
        [SerializeField]
        float healAmount = 30f;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviorComponent = gameObjectToAttachTo.AddComponent<SelfHealBeheviour>();
            behaviorComponent.SetConfig(this);

            base.behavior = behaviorComponent;
        }

        public float GetHealAmount()
        {
            return healAmount;
        }
    }
}