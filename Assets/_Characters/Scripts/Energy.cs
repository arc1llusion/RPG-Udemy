using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField]
        private Image energyOrb = null;

        [SerializeField]
        private float maxEnergyPoints = 100f;

        [SerializeField]
        private float regenPointsPerSecond = 1f;

        private float currentEnergyPoints = 100f;

        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
        }

        private void Update()
        {           
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
            }
        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
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
            float percentage = currentEnergyPoints / maxEnergyPoints;
            energyOrb.fillAmount = percentage;
        }
    }

}