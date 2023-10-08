using Player;
using UnityEngine;

namespace Interactables
{
    public class Pond : Interactable
    {

        public float waterStrength = 1;
        public float timeStrength = 30;
        
        public override void OnInteract(Frog frog)
        {
            switch (frog)
            {
                case WaterFrog waterFrog:
                    waterFrog.SetWater(waterFrog.water + this.waterStrength);
                    break;
                case TadpoleFrog:
                    Timer timer = FindObjectOfType<Timer>();
                    timer.AddSeconds(timeStrength);
                    
                    break;
            }
            
            Destroy(this.gameObject);
        }
    }
}