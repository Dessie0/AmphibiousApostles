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
                    if (!this.hasTadpole) break;
                    if (this.isWatered) break;
                    if (waterFrog.water < 0) break; 
                    
                    this.isWatered = true;
                    waterFrog.UseWater(1);
                    
                    break;
                case TadpoleFrog tadpoleFrog:
                    if (this.hasTadpole) break;
                    if (tadpoleFrog.tadpoleManager.tadpoles < 0) break;
                    
                    this.hasTadpole = true;
                    tadpoleFrog.UseTadpole();
                    break;
            }
        }
    }
}