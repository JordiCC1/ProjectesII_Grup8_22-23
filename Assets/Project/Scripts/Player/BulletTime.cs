using UnityEngine;
using System.Collections;


namespace Player
{

    public struct BulletTimeInputs
    {
        public bool BulletTimeDown;
        public bool BulletTimeUp;
        public bool SwapUp;
        public bool lastState;
    }

    public class BulletTime : MonoBehaviour
    {
        [Header("Bullet Time")]
        public float slowdownFactor = 0.2f;
        private float normalTimeScale = 1.0f;
        private float actualTimeScale = 1.0f;

        public bool isActive = false;
        bool hasStopped = false;

        [HideInInspector] public bool trailOn = false;

        public static BulletTime instance;

        public float timeToNormal = .75f;

        [SerializeField] StaminaController staminaController;

        [Header("Interpolation")]
        Interpolator lerp;
        public AnimationCurve curve;

        [Header("Audio")]
        public AudioClip enterSound;
        public AudioClip exitSound;


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

            //FinishBulletTime();

            lerp = new Interpolator(timeToNormal);
        }

        public void UpdateBulletTime(BulletTimeInputs inputs, bool Airborne)
        {
            if (inputs.SwapUp)
            {
                this.gameObject.GetComponentInChildren<Laser>().Shoot();
            }

            if (staminaController.stamina >= 0.0f)
            {
                if (inputs.BulletTimeDown)
                {
                    BulletTimeEffect.instance.StartEffect();
                    BulletTimeActive();
                    staminaController.UseStamina();
                }
                else if (inputs.BulletTimeUp)
                {
                    BulletTimeEffect.instance.StopEffect();
                    FinishBulletTime();
                    staminaController.StopStamina();
                }
            }
            else
            {
                BulletTimeEffect.instance.StopEffect();
                FinishBulletTime();
                staminaController.StopStamina();
            }

            if (!Airborne)
            {
                if(hasStopped)
                {
                    staminaController.StopStamina();
                    staminaController.ResetStamina();
                }

            }


        }
        void BulletTimeActive()
        {
            trailOn = true;
            Time.timeScale = slowdownFactor;
            actualTimeScale = slowdownFactor;
            isActive = true;
            SFXManager.instance.ChangePitch(0.5f);
            MusicManager.instance.ChangePitch(0.5f);
            SFXManager.instance.PlayAudio2D(this.transform, enterSound);
            hasStopped = false;
        }

        void FinishBulletTime()
        {
            trailOn = false;
            actualTimeScale = 1.0f;
            Time.timeScale = actualTimeScale;
            isActive = false;
            hasStopped = true;
            SFXManager.instance.ChangePitch(1.0f);
            MusicManager.instance.ChangePitch(1.0f);
            SFXManager.instance.PlayAudio2D(this.transform, exitSound);
        }

        public void BackToNormal()
        {
            lerp.Update(Time.deltaTime);

            if (lerp.IsMaxPrecise)
                lerp.ToMin();

            else if (lerp.IsMinPrecise)
                lerp.ToMax();

            actualTimeScale = Mathf.Lerp(actualTimeScale, normalTimeScale, curve.Evaluate(lerp.Value));
        }

    }
}
