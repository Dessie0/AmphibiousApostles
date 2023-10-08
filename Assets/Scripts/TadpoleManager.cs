using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TadpoleManager : MonoBehaviour
{
    public int numTadpoles;
    public GameObject defaultTadpole;
    public GameObject canvas;
    public Timer timer;
    
    private List<GameObject> tadpoles = new();

    public void Start()
    {
        if (canvas == null)
        {
            throw new Exception("No canvas!");
        }
        
        this.SetValue(this.numTadpoles);
    }
    
    public void SetValue(int value)
    {
        foreach(var tadpole in this.tadpoles)
        {
            Destroy(tadpole.gameObject);
        }
        
        this.tadpoles.Clear();
        this.numTadpoles = value;
        
        float y = 100;
        for (var i = 0; i < numTadpoles; i++)
        {
            GameObject duplicated = Instantiate(defaultTadpole, canvas.transform, false);

            RectTransform rectTransform = duplicated.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                //this is stupid
                rectTransform.localPosition = Vector3.zero;
                rectTransform.position = rectTransform.position + new Vector3(40f, y, 0f);
            }
            
            this.tadpoles.Add(duplicated);
            
            y -= 50;
        }
    }
}