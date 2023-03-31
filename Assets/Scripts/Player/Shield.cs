using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{    public struct ShieldInputs
    {
        public bool ShieldDown;
        public bool ShieldUp;
    }
    public class Shield : MonoBehaviour
    {
        public bool isShieldUp;
        public GameObject shield;
        [SerializeField] StaminaController staminaController;
        [SerializeField] private GameObject bullet;

        // Start is called before the first frame update
        void Start()
        {
            shield.SetActive(false);
            isShieldUp = false;
        }

        public void UpdateShield(ShieldInputs inputs)
        {
            if(inputs.ShieldDown && staminaController.stamina > 0.0f)
            {
                shield.SetActive(true);
                isShieldUp = true;
                staminaController.UseStamina();
            }
            else if(inputs.ShieldUp)
            {
                shield.SetActive(false);
                isShieldUp = false;
                if(Time.timeScale < 1.0f)
                    staminaController.StopStamina();
            }

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject objCollided = collision.gameObject;
            //GameObject aux = objCollided;
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa");
            if (objCollided.CompareTag("Bullet"))
            {
                Destroy(collision.gameObject);
                //objCollided = Instantiate(bullet, aux.transform.position, Quaternion.identity);
                //objCollided.GetComponent<Rigidbody2D>().AddForce(-aux.GetComponent<enemyBullet>().GetForce());
                //objCollided.GetComponent<enemyBullet>().SetForce(-aux.GetComponent<enemyBullet>().GetForce());
            }
        }
    }
}