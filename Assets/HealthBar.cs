using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public PlayerController player;

    public void Start() {
        slider = GetComponent<Slider>();
    }
    public void Update() {
        slider.value = player.hp;
    }
}
