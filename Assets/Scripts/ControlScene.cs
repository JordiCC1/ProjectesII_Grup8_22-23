using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScene : MonoBehaviour
{
    public int index;
    public string levelName;

    private CheckpointMaster cm;
    private ScreenWipe sw;

    private void OnTriggerEnter2D(Collider2D other)
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointMaster>();
        sw = FindObjectOfType<ScreenWipe>();
        if (other.CompareTag("Player")|| other.CompareTag("aPlayer"))
        {            
            cm.DestroyThis();
            sw.DestroyThis();
            StartCoroutine("Wait");
        }
    }

    IEnumerator Wait()
    {


        yield return new WaitForSeconds(0.7f);

        SceneManager.LoadScene(index);
    }
    
}