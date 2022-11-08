using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Swap : MonoBehaviour
{

    void Update()
    {
        //Check for mouse click 
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 point = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D raycastHit= Physics2D.Raycast(point, Vector2.zero);
            //CurrentClickedGameObject(raycastHit.transform.gameObject);
            //if(EventSystem.current.currentSelectedGameObject!=null)
            if (raycastHit.collider != null) {
                Debug.Log("hit");
                SwapGameObject(raycastHit.collider.gameObject);
            }
        }
    }
    
    public void SwapGameObject(GameObject Objective)
    {
        if (Objective.CompareTag("Enemy"))
        {
                Debug.Log("Enemy");
            Vector2 lastPosition = this.gameObject.transform.position;
            this.gameObject.transform.position = Objective.transform.position;
            Objective.transform.position = lastPosition;
        }
    }

    //private void OnMouseDown()
    //{
    //    GameObject Objective = EventSystem.current.currentSelectedGameObject;
    //    if (Objective != null)
    //    {
    //       SwapGameObject(Objective);
    //    }
    //}
}
