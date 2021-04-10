using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer rope; //The visuals of the rope
    [SerializeField] private SpringJoint2D springJoint; //The actual ACTING rope
    [SerializeField] private bool holdingLetter = false;
    [SerializeField] public float pickupRadius; //How far away you can pick up a letter

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rope = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();
        springJoint.enabled = false; //Disable the rope when we start so it doesn't cause issues
    }

    private void FixedUpdate()
    {
        Move();
        GrabLetter();
        if (holdingLetter)
        {
            UpdateRopeGFX(); //No need to show the visuals if there is no rope
        }
    }

    //Drone go brrr
    private void Move()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(hor, ver);
        Vector2 velocity = input * acceleration;
        
        rb.velocity = velocity * Time.deltaTime;
    }

    //Attach the springjoint to a letter within the predefined radius
    private void GrabLetter()
    {
        if (!holdingLetter && Input.GetKey(KeyCode.Space))
        {
            Collider2D[] letters = Physics2D.OverlapCircleAll(transform.position, pickupRadius); //Get everything with a collider within the radius

            //Go through them all
            for (int i = 0; i < letters.Length; i++) 
            {
                if (letters[i].tag.Equals("Letter")) //If you find a letter
                {
                    springJoint.enabled = true; //enable the rope
                    springJoint.connectedBody = letters[i].GetComponent<Rigidbody2D>(); //Connect the rope to the letter
                    letters[i].GetComponent<Letter>().letterThrown = true; //The letter has now been touched (for collision and scoring and etc)
                    holdingLetter = true; 
                    rope.enabled = true; //show the rope
                    return;
                }
            }
        }
        else if (holdingLetter && !Input.GetKey(KeyCode.Space))
        {
            StopEverything();
        }
    }

    //Update the position of the linerenderer so it accurately shows what we're holding
    private void UpdateRopeGFX()
    {
        rope.SetPosition(0, transform.position);
        rope.SetPosition(1, springJoint.connectedBody.transform.position);
    }

    //Stop all rope actions
    private void StopEverything()
    {
        springJoint.connectedBody = null;
        holdingLetter = false;
        springJoint.enabled = false;
        rope.enabled = false;
    }

    //What we want to happen when we deliver a letter
    public void DeliverLetter()
    {
        StopEverything();
    }
}
