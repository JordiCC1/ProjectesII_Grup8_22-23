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
        [Header("Bullet Time")]
        public float slowdownFactor = 0.2f;
        private float normalTimeScale = 1.0f;
        private float actualTimeScale = 1.0f;

		public bool isActive = false;
        bool hasStopped=false;

		public static BulletTime instance;

        public float timeToNormal=.75f;

        [SerializeField] PauseMenu pauseMenu;

        [Header("Interpolation")]
		Interpolator lerp;
		public AnimationCurve curve;

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

			lerp = new Interpolator(timeToNormal);
		}

        public void UpdateBulletTime(BulletTimeInputs inputs, bool canBT)
        {
            if (inputs.BulletTimeUp)
            {
                this.gameObject.GetComponentInChildren<Laser>().Shoot();
            }
            if (canBT)
            { 
                if (StaminaController.instance.stamina >= 0.0f)
                {
                    if (inputs.BulletTimeDown)
                    {
                        BulletTimeEffect.instance.StartEffect();
                        BulletTimeActive();
                        StaminaController.instance.UseStamina();
                    }
                    else if (inputs.BulletTimeUp)
                    {
                        BulletTimeEffect.instance.StopEffect();
                        FinishBulletTime();
                        StaminaController.instance.StopStamina();                        
                    }
                }
                else
                {
                    BulletTimeEffect.instance.StopEffect();
                    FinishBulletTime();
                    StaminaController.instance.StopStamina();
                    
                }
            }
            else
            {
                if (!hasStopped)
                {
                    FinishBulletTime();
                    BulletTimeEffect.instance.StopEffect();

                }
                StaminaController.instance.StopStamina();
            }

		}

        void BulletTimeActive()
        {            
            Time.timeScale = slowdownFactor;
            actualTimeScale = slowdownFactor;
            isActive = true;
            AudioManager.instance.ChangePitch(0.8f);
            hasStopped = false;
        }

        void FinishBulletTime()
        {
            actualTimeScale = 1.0f;
            Time.timeScale = actualTimeScale;
            isActive = false;
            StaminaController.instance.ResetStamina();
            hasStopped = true;
            AudioManager.instance.ChangePitch(1.0f);
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
