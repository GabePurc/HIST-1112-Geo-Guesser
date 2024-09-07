using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LocationScript : MonoBehaviour
{
    GameManager gameManager;

    public Color startingColor;

    private string location;
    private bool isCorrect, isGuessed;

    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        gameManager = GameManager.instance;
    }

    void Update()
    { 
            
        
        if (gameManager != null)
        {
            location = gameManager.location;
        }

        HighlightLocation();
    }

    private void OnMouseOver()
    {
        // Checks if player is on correct location

        if (location != null)
        {    
            if (location.Equals(gameObject.tag))
            {
                isCorrect = true;
            }
            else
            {
                isCorrect = false;
            }
        }

        // Inputs players guess
        if (Input.GetMouseButtonDown(0) && !isGuessed)
        {
            if (isCorrect)
            {
                setColor();
                isGuessed = true;
            }

            gameManager.PlayerGuessed(isCorrect);
        }

       
    }

    private void OnMouseEnter()
    {
        if (!isGuessed)
        {
            sprite.color += new Color(.4f, .4f, .4f);
        }
    }

    private void OnMouseExit()
    {
        if (!isGuessed)
        {
            sprite.color -= new Color(.4f, .4f, .4f);

            isCorrect = false;
        }
    }

    // Setting color of the sprite based on how many guesses it took the user
    private void setColor()
    {
        if (gameManager.currentGuesses >= 3)
        {
            sprite.color = new Color(1, 0, 0);
        }
        else if (gameManager.currentGuesses == 2)
        {
            sprite.color = new Color(.8f, .8f, 0);
        }
        else
        {
            sprite.color = new Color(0, 1, 0);
        }
    }

    public void ResetGame()
    {
        sprite.color = startingColor;
        isGuessed = false;
    }

    void HighlightLocation()
    {
        if (gameManager.currentGuesses >= 3 && tag.Equals(location) && !isGuessed)
        {
            sprite.color = new Color(0f, .5f, 1f);
        }

    }
}
