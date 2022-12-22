using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Player
{

    public class Laser : MonoBehaviour
    {
        public Transform laserFirePoint;
        public LineRenderer lineRenderer;
        Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }
        private void Update()
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.transform.right);
        }
        public void Shoot()
        {
            if (Physics2D.Raycast(_transform.position, transform.right))
            {
                RaycastHit2D hit = Physics2D.Raycast(laserFirePoint.position, transform.right);
                Draw2DRay(laserFirePoint.position, hit.point);
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<Controller>().OnSwap();
                    SwapGameObject(hit.collider.gameObject);
                    AudioManager.instance.PBulletEnemyCollisionSFX();
                }
            }
            Draw2DRay(laserFirePoint.position, laserFirePoint.transform.right);

        }

        private void Draw2DRay(Vector2 startPos, Vector2 endPos)
        {
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }

        public void SwapGameObject(GameObject Objective)
        {
            Vector3 lastPos = this.gameObject.transform.parent.position;
            Vector3 newPos = Objective.transform.position;
            Objective.GetComponent<Controller>().SwapAnimation(lastPos);
            this.gameObject.GetComponentInParent<Player>().Invincibility();
            this.gameObject.GetComponentInParent<Player>().isSwapped = true;
            this.gameObject.GetComponentInParent<Player>().targetPosition = newPos;
            StaminaController.instance.ResetStamina();
            BulletTime.instance.BackToNormal();
        }
    }
}
