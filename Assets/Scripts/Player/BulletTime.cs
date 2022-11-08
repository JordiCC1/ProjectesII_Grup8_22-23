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

        public void UpdateBulletTime(bool input, bool isActive)
        {
            if (input)
            {
                BulletTimeActive(isActive);
            }
            else
            {
                FinishBulletTime(isActive);
            }
        }

        //private void Start()
        //{
        //    isActive = false;
        //}

        //private void Update()
        //{
        //    if (Input.GetButton("Fire1"))
        //    {
        //        BulletTimeActive();
        //    }
        //    else
        //    {
        //        FinishBulletTime();
        //    }
        //}
    }
}
