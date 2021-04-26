using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("time").GetComponent<Text>().text = TimeHolder.getDuration().ToString();
        Restart();
    }
    void Restart() {
        if (Input.GetKeyDown(KeyCode.R)) {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    } 
}