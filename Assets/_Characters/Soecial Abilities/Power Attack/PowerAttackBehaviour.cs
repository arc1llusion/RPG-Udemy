using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : BaseBehaviour<PowerAttackConfig>
    {
        public override void Use(AbilityUseParams abilityParams)
        {
            float damageToDeal = abilityParams.baseDamage + config.GetExtraDamage();
            abilityParams.target.TakeDamage(damageToDeal);
        }
    }
}