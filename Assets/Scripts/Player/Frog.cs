using System;
using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public abstract class Frog : MonoBehaviour
    {
        
        //How close the player must be to the grid to be completed.
        [SerializeField]
        private float gridThreshold = 0.03f;
        public float speed = 0.3f;
        public LayerMask collisionMask;
        
        private Vector2 direction;
        private Vector2 targetPosition;
    
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        //Used in the editor for an animation event, tells the sprite when to actually move during the animation.
        private bool animationToggle;
        
        //Tracks if the player has enter an input, and whether the player is within the animation.
        //During the animation, the player cannot change the direction.
        private bool isMoving;
        
        //Used to track if the sprite should be flipped.
        //Cannot use facing as facing will flip the sprite when they move down.
        private bool isLookingLeft;
        
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
            if (!context.started) return;
            if (this.isMoving) return;
            
            this.direction = context.ReadValue<Vector2>();
            
            // Restrict diagonal movement
            this.RestrictDiagonal();
            
            //Set the animation bool if they're moving up so the appropriate animation plays.
            this.animator.SetInteger(Direction, Math.Abs(this.direction.y - 1f) < 0.01 ? 1 : 0);
            
            //Flip the sprite
            this.spriteRenderer.flipX = Math.Abs(this.direction.x - -1f) < 0.01 || !(Math.Abs(this.direction.x - 1) < 0.01) && this.spriteRenderer.flipX;
            
            //Check if the player can move
            if (!this.CheckCanMove(this.direction)) return;
            
            //We are now moving so don't accept any more inputs.
            this.animator.SetBool(IsMoving, true);
            this.isMoving = true;
        }
        
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            RaycastHit2D hit = this.RaycastForward(this.direction);
            if (hit.collider == null) return;
            
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            if(interactable == null) return;
            
            interactable.OnInteract(this);
        }
        
        private void Update()
        {
            Vector2 position = this.transform.position;
            Debug.DrawRay(new Vector3(position.x, position.y, 1f), new Vector3(this.direction.x, this.direction.y, 1f), Color.red);
        }
        
        private RaycastHit2D RaycastForward(Vector2 forwardDirection)
        {
            Vector2 origin = this.GetPosition();
            return Physics2D.Raycast(origin, forwardDirection, 1f, this.collisionMask);
        }
        
        private bool CheckCanMove(Vector2 forwardDirection)
        {
            return this.RaycastForward(forwardDirection).collider == null;
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
                //Tell the script that we've locked the frog into place and another movement can be inputted.
                this.isMoving = false;
                
                //Turn off the animation toggle, since the animation ended.
                this.animationToggle = false;
                
                //Lock the position to the tile
                this.transform.position = new Vector3(this.targetPosition.x, this.targetPosition.y, position.z);
                
                //Stop the animation.
                this.animator.SetBool(IsMoving, false);
            }
            else
            {
                //Slowly animate the position to the new location.
                this.transform.position = Vector3.Lerp(position, new Vector3(this.targetPosition.x, this.targetPosition.y, position.z), this.speed);
            }
        }
        
        private void AnimationShouldMove()
        {
            if (this.animationToggle) return;
            
            this.animationToggle = true;
            this.targetPosition = this.GetPosition() + new Vector2(this.direction.x, this.direction.y);
        }

        private void StopAnimation()
        {
            this.isMoving = false;
            this.animationToggle = false;
            this.animator.SetBool(IsMoving, false);
        }
    }
}
