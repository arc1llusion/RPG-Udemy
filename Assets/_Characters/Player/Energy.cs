using RPG.CameraUI;
using System.Collections;
using System.Collections.Generic;
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
            cameraRaycaster.notifyRightClickObservers += ProcessRightClick;
        }

        void ProcessRightClick(RaycastHit raycastHit, int layerHit)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - energyPerHit, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            float percentage = currentEnergyPoints / maxEnergyPoints;

            float xValue = -(percentage / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }

}