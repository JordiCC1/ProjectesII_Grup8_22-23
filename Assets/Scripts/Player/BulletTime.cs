using UnityEngine;
using System.Collections;

namespace Player
{
    public struct BulletTimeInputs
    {
        public bool BulletTimeDown;
        public bool BulletTimeUp;
        public bool lastState;
    }

    public class BulletTime : MonoBehaviour
    {
        public float slowdownFactor = 0.25f;

        void BulletTimeActive()
        {
            Time.timeScale = slowdownFactor;
            
        }

        void FinishBulletTime()
        {
            Time.timeScale = 1.0f;
           
        }

        public void UpdateBulletTime(BulletTimeInputs inputs, bool isActive, bool canBT)
        {
            
            if (canBT)
            {
                if (StaminaController.instance.stamina > 0)
                {
                    if (inputs.BulletTimeDown)
                    {
                        BulletTimeActive();
                        StaminaController.instance.UseStamina();
                    }
                    else if (inputs.BulletTimeUp)
                    {
                        this.gameObject.GetComponentInChildren<PlayerGun>().Shoot();
                        FinishBulletTime();
                        StaminaController.instance.StopStamina();
                    }
                }
            }
            else
            {
                FinishBulletTime();
                StaminaController.instance.StopStamina();
            }
        }

    }
}
