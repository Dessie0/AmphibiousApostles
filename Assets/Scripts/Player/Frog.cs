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
        
        private Vector2 direction;
        private Vector2 facing;
        private Vector2 targetPosition;
    
        private bool moving;
    
        // Start is called before the first frame update
        void Start()
        {
            this.targetPosition = this.GetPosition();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            this.moving = context.started || !context.canceled && this.moving;
            this.direction = context.ReadValue<Vector2>();

            // Restrict diagonal movement
            this.RestrictDiagonal();
            
            // Rotate the player in the movement direction.
            this.facing = this.direction;
            
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
            Vector2 origin = this.GetPosition();
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

        private Vector2 GetPosition()
        {
            Vector2 position = this.transform.position;
            return new Vector2(position.x, position.y);
        }
        
        // Update is called once per frame
        private void FixedUpdate()
        {
            Vector3 position = this.transform.position;
            
            if (Vector2.Distance(this.GetPosition(), this.targetPosition) < this.gridThreshold)
            {
                this.transform.position = new Vector3(this.targetPosition.x, this.targetPosition.y, position.z);
                
                if(!this.moving || !this.CheckCanMove()) return;
                this.targetPosition = this.GetPosition() + new Vector2(this.direction.x, this.direction.y);
            }
            else
            {
                this.transform.position = Vector3.Lerp(position, new Vector3(this.targetPosition.x, this.targetPosition.y, position.z), this.speed);
            }
        }
        
    }
}
