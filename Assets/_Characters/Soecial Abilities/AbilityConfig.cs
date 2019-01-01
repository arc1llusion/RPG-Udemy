using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
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

        [SerializeField] AudioClip[] audioClips = null;

        protected AbilityBeheviour behavior;

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AudioClip GetRandomAbilitySound()
        {
            var idx = UnityEngine.Random.Range(0, audioClips.Length);
            return audioClips[idx];
        }

        public void AttachAbilityTo(GameObject objectToAttachTo)
        {
            var behaviorComponent = GetBehaviourComponent(objectToAttachTo);

            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public abstract AbilityBeheviour GetBehaviourComponent(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams abilityParams)
        {
            behavior.Use(abilityParams);
        }
    }

}