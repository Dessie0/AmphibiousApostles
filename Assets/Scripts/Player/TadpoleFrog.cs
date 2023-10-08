using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class TadpoleFrog : Frog
    {
        
        public TadpoleManager tadpoleManager;


        protected override void Start()
        {
            base.Start();

            this.tadpoleManager = this.GetComponent<TadpoleManager>();
            if (this.tadpoleManager == null)
            {
                throw new Exception("Unable to find TadpoleManager!");
            }
        }

        public void UseTadpole()
        {
            this.tadpoleManager.tadpoles--;
            this.tadpoleManager.UpdateSprites();
        }
        
    }
}