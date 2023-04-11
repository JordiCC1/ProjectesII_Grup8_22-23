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
    private LevelLoader Ll;


    private void OnTriggerEnter2D(Collider2D other)
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointMaster>();
        sw = FindObjectOfType<ScreenWipe>();
        Ll = FindObjectOfType<LevelLoader>();
        if (other.CompareTag("Player")|| other.CompareTag("aPlayer"))
        {
            cm.DestroyThis();
            sw.DestroyThis();
            Ll.load = true;
            StartCoroutine("Wait");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(index);
    }

    
    
}