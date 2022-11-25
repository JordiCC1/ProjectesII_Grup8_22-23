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
        public bool isActive = false;

        public static BulletTime instance;

        private float actualTimeScale;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
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
                StaminaController.instance.StopStamina();
            }
        }

        void BulletTimeActive()
        {
            Time.timeScale = slowdownFactor;
            actualTimeScale = slowdownFactor;
            isActive = true;
            AudioManager.instance.ChangePitch(0.9f);
        }

        void FinishBulletTime()
        {
            Time.timeScale = 1.0f;
            actualTimeScale = 1.0f;
            isActive = false;
            StaminaController.instance.ResetStamina();
            AudioManager.instance.ChangePitch(1.0f);
        }

        public IEnumerator BackToNormalSpeed()
        {
            while (actualTimeScale < 1.0f)
            {
                yield return new WaitForSeconds(0.1f);
                actualTimeScale += 0.1f;
                Time.timeScale = actualTimeScale;
            }
            actualTimeScale = 1.0f;
            Time.timeScale = actualTimeScale;
        }

    }
}
