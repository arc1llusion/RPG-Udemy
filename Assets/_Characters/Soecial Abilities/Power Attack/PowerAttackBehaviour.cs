using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config = null;

        void Start()
        {

        }

        void Update()
        {

        }

        public void Use()
        {
            //throw new System.NotImplementedException();
        }

        internal void SetConfig(PowerAttackConfig powerAttackConfig)
        {
            config = powerAttackConfig;
        }
    }
}