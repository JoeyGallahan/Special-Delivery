using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public bool specialLetter = false;
    public bool letterThrown = false;
    private int points = 10;
    GameplayManager gameplay;

    private void Awake()
    {
        gameplay = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameplayManager>();
        if (specialLetter)
        {
            points *= 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Ground") && !letterThrown) //If the letter hits the ground and hasn't been touched by the player
        {
            Destroy(gameObject); //Kill it
        }
        if (collision.transform.tag.Equals("Mailbox") && letterThrown) //If the letter hits the mailbox and has been touched by the player
        {
            DeliverLetter(); //Deliver the letter
        }
    }

    private void DeliverLetter()
    {
        gameplay.DeliverLetter(points);
        Destroy(gameObject);
    }
}
