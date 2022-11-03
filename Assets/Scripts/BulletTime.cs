using UnityEngine;

namespace Player
{
    public class BulletTime : MonoBehaviour
    {
        public float slowdownFactor = 0.25f;
        public bool isActive{get; private set; }

        void BulletTimeActive()
        {
            Time.timeScale = slowdownFactor;
            isActive = true;
        }

        void FinishBulletTime()
        {
            Time.timeScale = 1.0f;
            isActive = false;
        }

        private void Start() 
        {
            isActive = false;
        }

        private void Update()
        {
            if (Input.GetButton("Jump"))
            {
                BulletTimeActive();
            }
            else
            {
                FinishBulletTime();
            }
        }
    }
}
