using System;
using System.Collections.Generic;
using Interactables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class WaterFrog : Frog
    {
        //How much water the Water frog can carry
        public float maxWater = 5; 
        
        //How much water the Water frog is carrying
        public float water;

        public WaterBar waterBar;
        
        protected override void Start()
        {
            base.Start();

            if (this.waterBar == null)
            {
                throw new Exception("No water bar attached!");
            }
            
            this.waterBar.SetValue(this.water / this.maxWater);
        }

        public void UseWater(int value)
        {
            this.water -= value;
            this.waterBar.SetValue(this.water / this.maxWater);
        }
        
        public void SetWater(float value)
        {
            this.water = value;
            this.waterBar.SetValue(this.water / this.maxWater);
        }
        
    }
}