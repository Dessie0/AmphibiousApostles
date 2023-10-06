using Player;

namespace Interactables
{
    public class Nest : Interactable
    {

        public bool hasTadpole;
        public bool isWatered;
        
        public override void OnInteract(Frog frog)
        {
            switch (frog)
            {
                case WaterFrog waterFrog:
                    if (this.isWatered) break;
                    if (waterFrog.water < 0) break; 
                    
                    this.isWatered = this.hasTadpole;
                    waterFrog.water--;
                    
                    break;
                case TadpoleFrog tadpoleFrog:
                    if (this.hasTadpole) break;
                    if (tadpoleFrog.tadpoles < 0) break; 
                    
                    this.hasTadpole = true;
                    tadpoleFrog.tadpoles--;
                    break;
            }
            
        }
    }
}