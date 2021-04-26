using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadText : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController player;
    Text text;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        text = GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.hp <= 0) {
            text.text = "Press '\"JUMP\" to wake up";
        } else {
            text.text = "";
        }
    }
}
