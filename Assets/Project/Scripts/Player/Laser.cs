using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using UnityEngine.EventSystems;

namespace Player
{

    public class Laser : MonoBehaviour
    {
        [SerializeField] float rayDist = 100;
        public Transform laserFirePoint;
        public LineRenderer lineRenderer;
        Transform _transform;
        [SerializeField] private GunTransform gT;
        RaycastHit2D hit;

        [Header("Audio")]
        public AudioClip enemyCollision;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        public void Shoot()
        {
            hit = Physics2D.Raycast(laserFirePoint.position, transform.right, Mathf.Infinity);


            if (hit.rigidbody == null)
                return;

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Controller>().OnSwap();
                SwapGameObject(hit.collider.gameObject);
                SFXManager.instance.PlayAudio2D(this.transform, enemyCollision);
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, laserFirePoint.position);
                lineRenderer.SetPosition(1, hit.point);
                Invoke("ClearLaser", 0.2f);
            }

        }
        private void ClearLaser()
        {
            lineRenderer.positionCount = 0;
        }
        private void OnDrawGizmos()
        {
            if (hit.rigidbody != null)
                Gizmos.DrawLine(laserFirePoint.position, hit.point);
            Gizmos.DrawLine(laserFirePoint.position, laserFirePoint.transform.right * rayDist);
        }

        public void SwapGameObject(GameObject Objective)
        {
            Vector3 lastPos = this.gameObject.transform.parent.position;
            Vector3 newPos = Objective.transform.position;
            if (this.gameObject.GetComponentInParent<Player>().alternative == false)
            {
                Objective.GetComponent<Controller>().SwapAnimation(lastPos);
            }
            this.gameObject.GetComponentInParent<Player>().Invincibility();
            this.gameObject.GetComponentInParent<Player>().isSwapped = true;
            this.gameObject.GetComponentInParent<Player>().targetPosition = newPos;
            BulletTime.instance.BackToNormal();
        }
    }
}
