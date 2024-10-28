using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{   
   [SerializeField] TextMeshProUGUI timerText;
   [SerializeField] float remainingTime;
    public GameObject levelFailPanel;
    public PlayerMovement playerController;

    void Update()
    {
        if(remainingTime > 0) {
            remainingTime -= Time.deltaTime;
        }

        else if (remainingTime <= 0) {
            remainingTime = 0;
            //Function to implement gameOver() can go here 
            timerText.color = Color.red;
            levelFailPanel.SetActive(true);
            playerController.canMove = false;
        }

        int mins = Mathf.FloorToInt(remainingTime / 60);
        int sec = Mathf.FloorToInt(remainingTime % 60);


        timerText.text = string.Format("{0:00}:{1:00}", mins, sec);
    }

    void updateTimer(float currentTime) {
        currentTime += 1;

        float mins = Mathf.FloorToInt(currentTime / 60);
        float sec = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", mins, sec);
    }
}
