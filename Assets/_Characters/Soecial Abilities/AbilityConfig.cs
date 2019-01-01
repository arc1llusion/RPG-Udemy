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
        public IDamageable caster;
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable caster, IDamageable target, float baseDamage)
        {
            this.caster = caster;
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField]
        float energyCost = 10f;

        [SerializeField] GameObject particlePrefab = null;

        [SerializeField] AudioClip audioClip = null;

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AudioClip GetAudioClip()
        {
            return audioClip;
        }

        protected ISpecialAbility behavior;

        public abstract void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams abilityParams)
        {
            behavior.Use(abilityParams);
        }
    }

}