

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBeheviour
    {
        public override void Use(AbilityUseParams abilityParams)
        {
            DealDamage(abilityParams);
            PlayParticleEffect();
            PlayAbilitySound();
        }

        private void DealDamage(AbilityUseParams abilityParams)
        {
            float damageToDeal = abilityParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage();
            abilityParams.target.TakeDamage(damageToDeal);
        }
    }
}