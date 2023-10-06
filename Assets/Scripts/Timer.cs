using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public int startingSeconds = 60;
    public bool timerRunning = true;
    public TextMeshProUGUI text;
    
    private float timeLeft = 60;

    private void Start()
    {
        this.timeLeft = this.startingSeconds;

        if (text == null)
        {
            throw new Exception("No display output for text.");
        }
        
    }

    public void AddSeconds(float seconds)
    {
        this.timeLeft += seconds;
    }

    private void Update()
    {
        if (!timerRunning) return;
        this.timeLeft -= Time.deltaTime;
        text.text = $"{Mathf.Floor(this.timeLeft / 60)}:{Math.Floor(this.timeLeft % 60)}";
    }
}