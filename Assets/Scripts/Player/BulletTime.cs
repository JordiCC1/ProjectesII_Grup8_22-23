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
        public bool isActive=false;

        public static BulletTime instance;

        private void Awake()
        {
            instance = this;
        }

        void BulletTimeActive()
        {
            Time.timeScale = slowdownFactor;
            isActive = true;
        }

        void FinishBulletTime()
        {
            Time.timeScale = 1.0f;
            isActive = false;
            StaminaController.instance.ResetStamina();
        }

        public void UpdateBulletTime(BulletTimeInputs inputs, bool canBT)
        {
            if (inputs.BulletTimeUp)
            {
                this.gameObject.GetComponentInChildren<Gun>().Shoot();
            }
            if (canBT)
            {
                if (StaminaController.instance.stamina >= 0.0f)
                {
                    if (inputs.BulletTimeDown)
                    {
                        BulletTimeActive();
                        StaminaController.instance.UseStamina();
                    }
                    else if (inputs.BulletTimeUp)
                    {
                        FinishBulletTime();
                        StaminaController.instance.StopStamina();
                    }
                }
                else
                {
                    FinishBulletTime();
                    StaminaController.instance.StopStamina();
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
