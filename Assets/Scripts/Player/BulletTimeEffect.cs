using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class BulletTimeEffect : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private float fillSpeed = 0.4f;
        [SerializeField] private float deFillSpeed = 0.8f;

        public static BulletTimeEffect instance;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            fillImage.fillAmount = 0;
        }
        public void StartEffect()
        {
            StopAllCoroutines();
            StartCoroutine("FillImage");
        }
        public void StopEffect()
        {
            StopAllCoroutines();
            StartCoroutine("DeFillImage");
        }

        IEnumerator FillImage()
        {
            while (fillImage.fillAmount < 1)
            {
                fillImage.fillAmount += fillSpeed * Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator DeFillImage()
        {
            while (fillImage.fillAmount > 0)
            {
                fillImage.fillAmount -= deFillSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
