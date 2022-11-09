using UnityEngine;

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
                    BulletTimeActive(isActive);
                }
                else if (exitInput)
                {
                    FinishBulletTime(isActive);
                }
            }
        }

    }
}
