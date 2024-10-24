using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBehaviour : GenericGravityEntityBehaviour
{
    // This constant is used to determine the offset from the player's centre and their feet
    private float EntityCentreToFeetOffset = 0.50f;

    // This "constant" is used to refer to the player's collider
    private BoxCollider2D entityCollider;

    private Rigidbody2D entityRigidBody2D;

    private bool squishing = false;
    private bool squishingOfWall = false;


    // On landing scuttle away (use the playerBehaviour code to detect when on ground)
    // Need to squish on any contact based on the wall angle


    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        entityCollider = gameObject.GetComponent<BoxCollider2D>();
        entityRigidBody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!squishing)
        {
            base.FixedUpdate();
        }
        else if (squishingOfWall)
        {
            Squish(true); 
        }
        else // squishing of floor
        {
            Squish(false);
            // potentially slide a lil instead of immediate squish
        }
    }

    // This function squashes the sprite of the rat on collision with walls and floor
    // Boinging refers to whether the squashing will rebound the rat of the surface in question
    private void Squish(bool Boinging)
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If colliding with the environment
        if (collision.gameObject.layer == 6)
        {
            squishing = true;
            entityRigidBody2D.gravityScale = 0;


            // If the player is on the top of the floor object
            if (CurrentlyOnTopOfWallOrFloor(collision.gameObject))
            {
                squishingOfWall = false;
            }
            else
            {
                squishingOfWall = true;
            }
        }
    }


    // MAY BE UNNECESSARY
    private void OnCollisionExit2D(Collision2D collision)
    {
        Collider2D environmentStillInCollision = Physics2D.OverlapBox(gameObject.transform.position, new Vector2(entityCollider.size.x * gameObject.transform.localScale.x, entityCollider.size.y * gameObject.transform.localScale.y), gameObject.transform.rotation.eulerAngles.z, LayerMask.GetMask("Floors and Walls"));

        // If no longer touching the environment, update isGrounded
        if (collision.gameObject.layer == 6 && !environmentStillInCollision)
        {
            //isGrounded = false;
        }
        // If still touching an environment object, see if on top of this object
        // If not, mark as no longer grounded
        else if (collision.gameObject.layer == 6)
        {
            if (!CurrentlyOnTopOfWallOrFloor(environmentStillInCollision.gameObject))
            {
                //isGrounded = false;
            }
        }
    }

    public bool CurrentlyOnTopOfWallOrFloor(GameObject WallOrFloor)
    {
        GenericFloorBehaviour FloorBehaviour = WallOrFloor.GetComponent<GenericFloorBehaviour>();

        return (gameObject.transform.position.y - EntityCentreToFeetOffset >= FloorBehaviour.JumpableSurfaceEquation(gameObject.transform.position.x));
    }

}
