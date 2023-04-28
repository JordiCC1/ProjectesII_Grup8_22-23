using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class StaminaController : MonoBehaviour
    {
        [Header("Stamina Main")]
        [SerializeField] private float maxStamina = 2.0f;
        [field:SerializeField] public float stamina { get; private set; }
        private bool coroutineActive;

        [Header("Stamian Drain")]
        [Range(0, 50)] [SerializeField] private float staminaDrain = 1f;

        [Header("Stamina UI")]
        public StaminaBar staminaBar;

        private void Start()
        {
            stamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
        }

        private void Update()
        {
            staminaBar.SetStamina(stamina);
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
                stamina -= staminaDrain*Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        public void StopStamina()
        {
            StopAllCoroutines();
            coroutineActive = false;

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