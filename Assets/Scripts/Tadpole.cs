using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tadpole : MonoBehaviour
{
    public List<Sprite> spriteStages = new();
    public int spriteStage = 0;
    public Image imageRenderer;
        
    private void Update()
    {
        imageRenderer.sprite = spriteStages[spriteStage];
    }
}