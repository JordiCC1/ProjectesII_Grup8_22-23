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
                        //BulletTimeEffect.instance.StartEffect();

                    }
                    else if (inputs.BulletTimeUp)
                    {
                        FinishBulletTime();
                        StaminaController.instance.StopStamina();
                        //BulletTimeEffect.instance.StopEffect();
                    }
                }
                else
                {
                    FinishBulletTime();
                    StaminaController.instance.StopStamina();
                    //BulletTimeEffect.instance.StopEffect();
                }
            }
            else
            {
                StaminaController.instance.StopStamina();
                //BulletTimeEffect.instance.StopEffect();
            }

		}

        void BulletTimeActive()
        {
            Time.timeScale = slowdownFactor;
            actualTimeScale = slowdownFactor;
            isActive = true;
            AudioManager.instance.ChangePitch(0.8f);
        }

        void FinishBulletTime()
        {
            actualTimeScale = 1.0f;
            Time.timeScale =actualTimeScale;
            isActive = false;
            StaminaController.instance.ResetStamina();
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
