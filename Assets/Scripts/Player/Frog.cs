using System;
using System.Runtime.CompilerServices;
using Interactables;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    public abstract class Frog : MonoBehaviour
    {
        
        //How close the player must be to the grid to be completed.
        [SerializeField]
        private float gridThreshold = 0.01f;
        public float speed = 0.3f;
        public LayerMask collisionMask;
        
        private Rigidbody2D rig;
        private Vector2 direction;
        private Vector2 facing;
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
            
            if(!context.started) return;

            // Restrict diagonal movement
            this.RestrictDiagonal();
            
            // Rotate the player in the movement direction.
            this.facing = this.direction;
            float angle = Mathf.Atan2(this.facing.y, this.facing.x) * Mathf.Rad2Deg;
            //Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            //transform.rotation = targetRotation;
            
            //Check if there's a collider in front of the player.
            this.moving = CheckCanMove();
        }
    
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            RaycastHit2D hit = this.RaycastForward();
            if (hit.collider == null) return;
            
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if(interactable == null) return;
                
            interactable.OnInteract(this);
        }

        private void Update()
        {
            Vector2 position = this.transform.position;
            Debug.DrawRay(new Vector3(position.x, position.y, 1f), new Vector3(this.facing.x, this.facing.y, 1f), Color.red);
        }

        private RaycastHit2D RaycastForward()
        {
            Vector2 position = this.transform.position;
            Vector2 origin = new Vector2(position.x, position.y);
            return Physics2D.Raycast(origin, this.facing, 1f, this.collisionMask);
        }

        private bool CheckCanMove()
        {
            return this.RaycastForward().collider == null;
        }

        private void RestrictDiagonal()
        {
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
        
        // Update is called once per frame
        void FixedUpdate()
        {
            if (Vector2.Distance(this.rig.position, this.targetPosition) < this.gridThreshold)
            {
                this.rig.position = this.targetPosition;
                
                if (!this.moving) return;
                this.RestrictDiagonal();
                this.targetPosition = this.rig.position + new Vector2(this.direction.x, this.direction.y);
            }
            else
            {
                this.rig.position = Vector2.Lerp(this.rig.position, this.targetPosition, this.speed);
            }
        }
        
    }
}
