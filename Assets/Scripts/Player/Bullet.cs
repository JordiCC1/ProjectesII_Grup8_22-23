using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Enemy;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 4.5f;
        private Vector3 mousePos;
        private Camera mainCam;
        private Rigidbody2D rb;
        public float lifeTime;

        private void Start()
        {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            rb = GetComponent<Rigidbody2D>();
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            Vector3 rotation = transform.position - mousePos;
            rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
            float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot + 90);
            Destroy(gameObject, lifeTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject objCollided = collision.gameObject;
            if (objCollided.CompareTag("Enemy"))
            {
                objCollided.GetComponent<Controller>().OnSwap();                
                SwapGameObject(objCollided);
                AudioManager.instance.PBulletEnemyCollisionSFX();
            }
            else
                AudioManager.instance.PBulletWallCollisionSFX();
            Destroy(gameObject);            
        }

        public void SwapGameObject(GameObject Objective)
        {
            Vector3 lastPos = this.gameObject.transform.parent.position;
            Vector3 newPos = Objective.transform.position;
            this.gameObject.transform.parent.position = newPos;
            Objective.transform.position = lastPos;
            this.gameObject.GetComponentInParent<Player>().isInvincible = true;
            StaminaController.instance.ResetStamina();
            BulletTime.instance.BackToNormal();
            this.gameObject.GetComponentInParent<Player>().WarpCamera(lastPos);
        }
    }

}