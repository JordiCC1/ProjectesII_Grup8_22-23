using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScene : MonoBehaviour
{
    public int index;
    public string levelName;

    private CheckpointMaster cm;

    private void OnTriggerEnter2D(Collider2D other)
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointMaster>();
        if (other.CompareTag("Player")|| other.CompareTag("aPlayer"))
        {
            Debug.Log("destroyed");
            cm.DestroyThis();
            StartCoroutine("Wait");
        }
    }

    IEnumerator Wait()
    {


        yield return new WaitForSeconds(0.7f);

        SceneManager.LoadScene(index);
    }
    
}