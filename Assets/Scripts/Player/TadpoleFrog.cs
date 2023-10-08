using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class TadpoleFrog : Frog
    {
        
        //How many tadpoles the Tadpole frog can carry
        public int maxTadpoles = 5;
        
        //How many tadpoles the Tadpole frog is carrying
        public int tadpoles;

        private TadpoleManager tadpoleManager;


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
            this.tadpoles--;
            this.tadpoleManager.SetValue(this.tadpoles);
        } 
        
    }
}