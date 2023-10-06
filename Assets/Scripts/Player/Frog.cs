using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    public class Frog : MonoBehaviour
    {
        [SerializeField]
        private int player;
        
        [SerializeField]
        private float speed = 0.3f;
    
        //How close the player must be to the grid to be completed.
        [SerializeField]
        private float gridThreshold = 0.01f;

        private Rigidbody2D rig;
        private Vector2 direction;
        private Vector2 targetPosition;
    
        private bool moving;
    
        // Start is called before the first frame update
        void Start()
        {
            this.rig = this.GetComponent<Rigidbody2D>();
            this.targetPosition = this.rig.position;

            if (this.rig == null)
            {
                throw new Exception("Unable to find Rigidbody for Frog.");
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            this.moving = context.started || !context.canceled && this.moving;
            this.direction = context.ReadValue<Vector2>();
        
            // Restrict diagonal movement
            if (this.direction.x == 0f || this.direction.y == 0f) return;
        
            if (Mathf.Abs(this.direction.x) > Mathf.Abs(this.direction.y))
            {
                // Zero out vertical movement, set horizontal to full throttle.
                this.direction.y = 0f;
                this.direction.x = Mathf.Round(this.direction.x);
            }
            else
            {
                // Zero out horizontal movement, set vertical to full throttle.
                this.direction.x = 0f; 
                this.direction.y = Mathf.Round(this.direction.y);
            }

        }
    
        public void OnInteract(InputAction.CallbackContext context)
        {
        
        }
    
        public bool IsTadpoleFrog()
        {
            return this.player == 1;
        }
    
        // Update is called once per frame
        void FixedUpdate()
        {
            if (Vector2.Distance(this.rig.position, this.targetPosition) < this.gridThreshold)
            {
                this.rig.position = this.targetPosition;
                if (this.moving)
                {
                    this.targetPosition = this.rig.position + new Vector2(this.direction.x, this.direction.y);
                }
            }
            else
            {
                this.rig.position = Vector2.Lerp(this.rig.position, this.targetPosition, this.speed);
            }
        }
    }
}
