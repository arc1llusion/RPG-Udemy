using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class BaseBehaviour<T> : MonoBehaviour, ISpecialAbility
    {
        protected T config = default(T);

        public void SetConfig(T attackConfig)
        {
            config = attackConfig;
        }

        public abstract void Use(AbilityUseParams abilityParams);
    }
}