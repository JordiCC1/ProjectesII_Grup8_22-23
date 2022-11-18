using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class StaminaController : MonoBehaviour
    {
        [Header("Stamina Main")]
        [SerializeField] private float maxStamina = 3.0f;
        [field:SerializeField] public float stamina { get; private set; }
        private bool coroutineActive;

        [Header("Stamian Drain")]
        [Range(0, 50)] [SerializeField] private float staminaDrain = 1f;

        [Header("Stamina UI")]
        public Slider staminaBar;

        public static StaminaController instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            stamina = maxStamina;
            staminaBar.maxValue = maxStamina-1;
            staminaBar.value = maxStamina-1;
        }

        private void Update()
        {
            staminaBar.value = stamina-1;
        }
        #region UseStamina
        public void UseStamina()
        {
            if (!coroutineActive)
            {
                coroutineActive = true;
                StartCoroutine(DrainStamina());
            }
        }

        private IEnumerator DrainStamina()
        {
            while (stamina>=0.0f)
            {
                if (stamina - staminaDrain >= 0.0f)
                {
                    stamina -= staminaDrain*Time.deltaTime;
                    Debug.Log("--");
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public void StopStamina()
        {
            StopAllCoroutines();
            coroutineActive = false;
            Debug.Log("sTOP");
        }
        #endregion
        #region ResetStamina
        //Resets the stamina to max stamina
        public void ResetStamina()
        {
            stamina = maxStamina;
        }

        //Adds an amount of stamina back to the stamina
        public void AddStamina(int value)
        {
            stamina += value;
        }
        #endregion
    }
}