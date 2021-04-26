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
        GameObject.Find("time").GetComponent<Text>().text = "Your time was:\n" + TimeHolder.getDuration().ToString() + "s";
        Restart();
    }
    void Restart() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    } 
}
