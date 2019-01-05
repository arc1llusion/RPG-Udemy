using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {

        [SerializeField] AbilityConfig[] abilities = null;
        [SerializeField] private Image energyBar = null;
        [SerializeField] private float maxEnergyPoints = 100f;
        [SerializeField] private float regenPointsPerSecond = 1f;
        [SerializeField] AudioClip outOfEnergySound = null;

        private float currentEnergyPoints = 100f;

        AudioSource audioSource = null;

        float EnergyAsPercent
        {
            get
            {
                return currentEnergyPoints / maxEnergyPoints;
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
            audioSource = GetComponent<AudioSource>();

            AttachInitialAbilities();
            UpdateEnergyBar();
        }

        private void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
            }
        }

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyCost < currentEnergyPoints)
            {
                ConsumeEnergy(energyCost);
                abilities[abilityIndex].Use(target);
            }
            else
            {
                if (outOfEnergySound != null)
                {
                    audioSource.PlayOneShot(outOfEnergySound);
                }
            }
        }

        private void AttachInitialAbilities()
        {
            foreach (var ability in abilities)
                ability.AttachAbilityTo(gameObject);
        }

        public void AddEnergyPoints()
        {
            var regenThisFrame = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + regenThisFrame, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }

        public void ConsumeEnergy(float amount)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - amount, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            energyBar.fillAmount = EnergyAsPercent;
        }
    }

}