using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Player
{
    public class BulletTimeEffect : MonoBehaviour
    {
        [SerializeField] private GameObject fillImage;
        [SerializeField] private float fillSpeed = 0.4f;
        [SerializeField] private float deFillSpeed = 0.8f;


        private Vector3 originalScale;
        private Vector3 scaleTo;

        public static BulletTimeEffect instance;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            originalScale = fillImage.transform.localScale;
            scaleTo = originalScale * 1000f;
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
            fillImage.transform.DOScale(scaleTo, fillSpeed);
            yield return null;
            
        }

        IEnumerator DeFillImage()
        {
            transform.DOScale(originalScale, deFillSpeed);
            yield return null;            
        }
    }
}
