using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public interface ISpecialAbility
    {
        void Use(AbilityUseParams abilityParams);
    }

    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class SpecialAbility : ScriptableObject
    {

        [Header("Special Ability General")]
        [SerializeField]
        float energyCost = 10f;

        public float GetEnergyCost()
        {
            return energyCost;
        }

        protected ISpecialAbility behavior;

        public abstract void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams abilityParams)
        {
            behavior.Use(abilityParams);
        }
    }

}