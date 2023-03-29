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
                staminaController.UseStamina();
                isShieldUp = true;
            }
            else if(inputs.ShieldUp)
            {
                shield.SetActive(false);
                staminaController.StopStamina();
                isShieldUp = false;
            }

        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject objCollided = collision.gameObject;
            if (objCollided.CompareTag("Bullet"))
            {
                Vector2 aux = objCollided.GetComponent<enemyBullet>().GetForce();
                aux.x -= 2 * aux.x;
                aux.y -= 2 * aux.y;
                objCollided.GetComponent<Rigidbody2D>().AddForce(aux);
            }
        }
    }
}