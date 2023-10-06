using Player;
using UnityEngine;

namespace Interactables
{
    public class Pond : Interactable
    {
        public override void OnInteract(Frog frog)
        {
            switch (frog)
            {
                case WaterFrog waterFrog:
                    waterFrog.water = waterFrog.maxWater;
                    break;
                case TadpoleFrog tadpoleFrog:
                    //TODO Add timer 
                    break;
            }
        }
    }
}