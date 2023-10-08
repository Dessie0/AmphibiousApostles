using System;
using Interactables;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        //Used in the editor for an animation event, tells the sprite when to actually move during the animation.
        private bool animationToggle;

        //Used to track if the sprite should be flipped.
        //Cannot use facing as facing will flip the sprite when they move down.
        private bool isLookingLeft;
        
        private bool moving;
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Direction = Animator.StringToHash("Direction");

        // Start is called before the first frame update
        protected virtual void Start()
        {
            this.targetPosition = this.GetPosition();
            this.animator = this.GetComponent<Animator>();
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            
            if (this.animator == null)
            {
                throw new Exception("Animator does not exist!");
            }
            
            if (this.spriteRenderer == null)
            {
                throw new Exception("SpriteRenderer does not exist!");
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            this.moving = context.started || !context.canceled && this.moving;
            this.direction = context.ReadValue<Vector2>();

            // Restrict diagonal movement
            this.RestrictDiagonal();
            
            // Rotate the player in the movement direction.
            if (!context.canceled)
            {
                this.facing = this.direction;
            }
            
            //Set the animation bool if they're moving up so the appropriate animation plays.
            this.animator.SetBool(IsMoving, this.moving);
            this.animator.SetInteger(Direction, Math.Abs(this.facing.y - 1f) < 0.01 ? 1 : 0);
            
            //Flip the sprite
            this.spriteRenderer.flipX = Math.Abs(this.direction.x - -1f) < 0.01 || !(Math.Abs(this.direction.x - 1) < 0.01) && this.spriteRenderer.flipX;
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
            if (!this.animationToggle) return;
            
            Vector3 position = this.transform.position;
            //Check if the player has reached their target position of the next tile
            if (Vector2.Distance(this.GetPosition(), this.targetPosition) < this.gridThreshold)
            {
                //Lock the position to the tile
                this.transform.position = new Vector3(this.targetPosition.x, this.targetPosition.y, position.z);
                
                //Check if they're still moving, or if they even CAN move in the first place
                if(!this.moving || !this.CheckCanMove())
                {
                    this.moving = false;
                    this.animator.SetBool(IsMoving, false);
                }
                else
                {

                    this.animationToggle = false;
                    
                    //Otherwise, set the next target position.
                    this.targetPosition = this.GetPosition() + new Vector2(this.direction.x, this.direction.y);
                }
            }
            else
            {
                //Slowly animate the position to the new location.
                this.transform.position = Vector3.Lerp(position, new Vector3(this.targetPosition.x, this.targetPosition.y, position.z), this.speed);
            }
        }


        private void AnimationShouldMove()
        {
            this.animationToggle = true;
        }
        
        
    }
}
