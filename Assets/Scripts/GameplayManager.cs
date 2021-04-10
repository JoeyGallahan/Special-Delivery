using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] Drone drone;

    //UI
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI gameOverScoreText;
    [SerializeField] GameObject gameOverScreen;

    //Letters to spawn
    [SerializeField] GameObject letterPrefab;
    [SerializeField] GameObject specialLetterPrefab;
    [SerializeField] float percentForSpecial; //The chance a special letter will drop

    //Cooldowns for dropping letts
    [SerializeField] float maxLetterCooldown = 1.0f;
    [SerializeField] float curLetterCooldown = 0.0f;
    [SerializeField] float letterCooldownElapsed = 0.0f;

    //Where the letters will drop from
    [SerializeField] Collider2D letterDropArea;

    [SerializeField] float timeLimit = 60.0f;
    [SerializeField] private int score = 0;

    private void Awake()
    {
        Time.timeScale = 1.0f; //Because reloading the scene doesn't reset the timescale
    }

    void Update()
    {
        Cooldown();
        TimeLimit();
    }

    //How long we have left in the game
    private void TimeLimit()
    {
        timeLimit -= Time.deltaTime;
        timeText.SetText(((int)timeLimit).ToString());

        if (timeLimit <= 0.0f)
        {
            timeLimit = 0.0f;
            GameOver();
        }
    }

    //When the time expires
    private void GameOver()
    {
        gameOverScoreText.SetText("Score: " + score.ToString()); //Update the final score text
        gameOverScreen.SetActive(true); //Show the game over UI
        Time.timeScale = 0f; //Pause the game
    }

    //Spawns a letter at a random location above the screen
    private void DropLetter()
    {
        Vector2 min = letterDropArea.bounds.min; 
        Vector2 max = letterDropArea.bounds.max;

        Vector2 dropPos = new Vector2(Random.Range(min.x, max.x), min.y); //Get a random position within the drop zone to spawn a letter

        GameObject letter;

        float special = Random.Range(0.0f, 1.0f);
        if (special <= percentForSpecial)
        {
            letter = Instantiate(specialLetterPrefab);  //Spawn a special letter
        }
        else
        {
            letter = Instantiate(letterPrefab); //Spawn a normal letter
        }
        letter.transform.position = dropPos; //Move it where we want it
    }

    //Time between letters dropping
    private void Cooldown()
    {
        letterCooldownElapsed += Time.deltaTime;

        if (letterCooldownElapsed >= maxLetterCooldown)
        {
            DropLetter();
            curLetterCooldown = Random.Range(0.0f, maxLetterCooldown);
            letterCooldownElapsed = 0.0f;
        }
    }

    //When the player throws a letter into the mailbox
    public void DeliverLetter(int points)
    {
        score += points; //Give em points
        scoreText.SetText(score.ToString()); //Update the ui text
        drone.DeliverLetter(); //Tell the drone we delivered a letter
    }

    //Restart the scene so we can play again
    public void Replay()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
