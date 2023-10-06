using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class Minimap : MonoBehaviour
    {
        public Tilemap tilemap;
        private Camera minimapCamera;

        private void Start()
        {
            this.minimapCamera = this.GetComponent<Camera>();

            if (this.minimapCamera == null)
            {
                throw new Exception("Could not find camera!");
            }
        }
    }
}