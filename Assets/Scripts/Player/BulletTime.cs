using UnityEngine;
using System.Collections;

namespace Player
{
    public struct BulletTimeInputs
    {
        public bool BulletTimeDown;
        public bool BulletTimeUp;
    }

    public class BulletTime : MonoBehaviour
    {
        public float slowdownFactor = 0.25f;
        //public bool isActive { get; private set; }

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
                if (inputs.BulletTimeDown)
                {
                    BulletTimeActive();
                }
                else if (inputs.BulletTimeUp)
                {
                    this.gameObject.GetComponentInChildren<PlayerGun>().Shoot();
                    FinishBulletTime();
                }
            }
            else
            {
                FinishBulletTime();
            }
        }

    }
}
