
using UnityEngine;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        private void DestroyChildren()
        {
            foreach(Transform t in transform)
            {
                DestroyImmediate(t.gameObject);
            }
        }

        private void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(pickUpSFX);
            }
            FindObjectOfType<Player>().PutWeaponInHand(weaponConfig);

            //Destroy(this.gameObject);
        }
    }

}