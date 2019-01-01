using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBeheviour : BaseBehaviour<SelfHealConfig>
    {
        public override void Use(AbilityUseParams abilityParams)
        {
            abilityParams.caster.TakeDamage(-config.GetHealAmount());
        }
    }
}