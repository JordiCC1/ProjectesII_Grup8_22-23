using UnityEngine;
using System.Collections;

namespace Player
{
    public class BulletTime : MonoBehaviour
    {
        public float slowdownFactor = 0.25f;
        //public bool isActive { get; private set; }

        void BulletTimeActive(bool isActive)
        {
            Time.timeScale = slowdownFactor;
        }

        void FinishBulletTime(bool isActive)
        {
            Time.timeScale = 1.0f;
        }

        public void UpdateBulletTime(bool enterInput, bool isActive, bool exitInput, bool canBT)
        {
            if (canBT)
            {
                if (enterInput)
                {
                    CinemachineShake.Instance.ShakeCamera(7f, 3f);
                    BulletTimeActive(isActive);
                }
                else if (exitInput)
                {
                    CinemachineShake.Instance.ShakeCamera(5f, .1f);
                    this.gameObject.GetComponentInChildren<PlayerGun>().Shoot();
                    FinishBulletTime(isActive);
                }
            }
            else
            {
                FinishBulletTime(isActive);
            }
        }

    }
}
