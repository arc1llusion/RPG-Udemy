using RPG.CameraUI;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField]
        private RawImage energyBar = null;

        [SerializeField]
        private float maxEnergyPoints = 100f;

        [SerializeField]
        private float energyPerHit = 10f;

        private float currentEnergyPoints = 100f;
        private CameraRaycaster cameraRaycaster;

        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;

            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButtonDown(1))
            {
                UpdateEnergyPoints();
                UpdateEnergyBar();
            }
        }

        private void UpdateEnergyPoints()
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - energyPerHit, 0f, maxEnergyPoints);
        }

        private void UpdateEnergyBar()
        {
            float percentage = currentEnergyPoints / maxEnergyPoints;

            float xValue = -(percentage / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }

}