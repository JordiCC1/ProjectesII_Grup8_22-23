using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Player
{
    
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        [SerializeField] private CinemachineVirtualCamera vCam1;
        private CinemachineComponentBase _myCamera;
        
        void Start()
        {
            _myCamera = vCam1.GetCinemachineComponent<CinemachineComponentBase>();
        }

        // Update is called once per frame
        public void UpdateCamera(Transform target, Vector3 positionDelta)
        {
            _myCamera.OnTargetObjectWarped(target, positionDelta);
            Debug.Log("swapped");
        }
    }
}
