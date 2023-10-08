using System;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleManager : MonoBehaviour
{
    //How many tadpoles the Tadpole frog is carrying
    public int tadpoles = 5;
    
    public GameObject defaultTadpole;
    public GameObject canvas;
    public Timer timer;

    [HideInInspector]
    public List<Tadpole> tadpoleSprites = new();

    public void Start()
    {
        if (canvas == null)
        {
            throw new Exception("No canvas!");
        }
        
        this.UpdateSprites();
    }
    
    public void UpdateSprites()
    {
        foreach(var tadpole in this.tadpoleSprites)
        {
            Destroy(tadpole.gameObject);
        }
        
        this.tadpoleSprites.Clear();
        
        float y = 100;
        for (var i = 0; i < this.tadpoles; i++)
        {
            GameObject duplicated = Instantiate(defaultTadpole, canvas.transform, false);

            RectTransform rectTransform = duplicated.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                //this is stupid
                rectTransform.localPosition = Vector3.zero;
                rectTransform.position = rectTransform.position + new Vector3(40f, y, 0f);
            }
            
            this.tadpoleSprites.Add(duplicated.GetComponent<Tadpole>());
            
            y -= 50;
        }
    }
}