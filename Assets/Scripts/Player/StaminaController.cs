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
            //staminaBar.GetComponentInChildren<StaminaBar>().SetMaxStamina(stamina-1);
            //staminaBar.SetActive(false);
        }

        private void Update()
        {
            staminaBar.SetStamina(stamina);
            //if (stamina == maxStamina && staminaBar.activeInHierarchy)
            //    staminaBar.SetActive(false);
        }

        #region UseStamina
        public void UseStamina()
        {
            if (!coroutineActive)
            {
                coroutineActive = true;
                StartCoroutine(DrainStamina());
                //if (!staminaBar.activeInHierarchy)
                //{
                //    staminaBar.SetActive(true);
                //    Debug.Log("´---sdfasfew");
                //}                
            }
        }

        private IEnumerator DrainStamina()
        {
            while (stamina>=0.0f)
            {
                //if (stamina - staminaDrain >= 1.0f)
               // {
                    stamina -= staminaDrain*Time.deltaTime;                    
               // }
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