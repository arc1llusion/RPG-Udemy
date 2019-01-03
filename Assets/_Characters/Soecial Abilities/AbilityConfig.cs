using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{

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

        public void Use(GameObject target)
        {
            behavior.Use(target);
        }
    }

}