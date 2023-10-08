using System;
using System.Collections.Generic;
using Interactables;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public int startingSeconds = 60;
    public bool timerRunning = true;
    public TextMeshProUGUI text;
    
    private float timeLeft = 60;
    private TadpoleFrog frog;

    private List<Nest> nests;

    private void Start()
    {
        this.frog = FindObjectOfType<TadpoleFrog>();
        this.timeLeft = this.startingSeconds;
        this.nests = new List<Nest>(FindObjectsOfType<Nest>());

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
        text.text = $"{Mathf.Floor(this.timeLeft / 60)}:{(this.timeLeft % 60):00}";

        //Reload the scene if they lose.
        if (this.timeLeft == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (this.timeLeft < (float) this.startingSeconds / 2)
        {
            foreach (var tadpole in this.frog.tadpoleManager.tadpoleSprites)
            {
                tadpole.spriteStage = 1;
            } 
        }

        if (this.timeLeft < (float) this.startingSeconds / 4)
        {
            foreach (var tadpole in this.frog.tadpoleManager.tadpoleSprites)
            {
                tadpole.spriteStage = 2;
            } 
        }
        
        //Check if they won
        foreach (var nest in this.nests)
        {
            if(!nest.hasTadpole || !nest.isWatered) continue;
            
            //Otherwise they won the level
            String activeScene = SceneManager.GetActiveScene().name;

            switch (activeScene) 
            {
                case "Level1": SceneManager.LoadScene("Level2");
                    break;
                case "Level2": SceneManager.LoadScene("Level3");
                    break;
                case "Level3": SceneManager.LoadScene("GameOver");
                    break;
            }
        }
        
    }
}