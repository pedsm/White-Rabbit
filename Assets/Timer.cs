using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    float startTime;
    float endTime = -1;
    float firstSectionDuration;
    Text textField;
    float flipTime = -1;

    void Start()
    {
        //Remove the -1000 when the game is done
        startTime = Time.time - 100;
        textField = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(endTime != -1) {
            textField.text = Mathf.RoundToInt(getFinalTime()).ToString();
            return;
        }
        if(flipTime == -1) {
            textField.text = Mathf.RoundToInt((Time.time - startTime)).ToString();
        } else {
            int timeLeft = Mathf.RoundToInt(firstSectionDuration - ((Time.time - flipTime) * 2));
            textField.text = timeLeft.ToString();
            if(timeLeft <= 0) {
                GameObject.Find("Player").GetComponent<PlayerController>().takeDamage(100);
            }
        }
    }

    public void flip() {
        flipTime = Time.time;
        firstSectionDuration = Time.time - startTime;
    }

    public void finishGame() {
        endTime = Time.time;
    }

    public float getFinalTime() {
        return endTime - startTime;
    }
}
