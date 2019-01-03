using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthBar = null;

        [SerializeField] AudioClip[] damageSounds = null;
        [SerializeField] AudioClip[] deathSounds = null;

        [SerializeField] float deathVanishSeconds = 2f;

        const string DEATH_TRIGGER = "Death";

        float currentHealthPoints;
        Animator animator;
        private AudioSource audioSource;

        Character characterMovement;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<Character>();

            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }

        public void TakeDamage(float damage)
        {
            bool characterDies = (currentHealthPoints - damage) <= 0;

            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

            if (damageSounds.Length > 0)
            {
                var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
                audioSource.PlayOneShot(clip);
            }

            if (characterDies)
            {
                StartCoroutine(KillCharacter());
            }
        }

        public void Heal(float amount)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + amount, 0f, maxHealthPoints);
        }

        IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            characterMovement.Kill();
            animator.SetTrigger(DEATH_TRIGGER);


            var playerComponent = GetComponent<PlayerMovement>();
            if(playerComponent && playerComponent.isActiveAndEnabled)
            {
                if (deathSounds.Length > 0)
                {
                    audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                    audioSource.Play();
                }

                yield return new WaitForSecondsRealtime(audioSource.clip.length);

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Destroy(gameObject, deathVanishSeconds);
            }
        }
    }
}