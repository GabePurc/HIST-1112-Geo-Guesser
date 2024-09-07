using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int currentIndex = 0;
    private int correctGuesses = 0;
    private int[] finalTime = new int[2];
    private float seconds = 0;

    public int currentGuesses = 0;
    public string location;
    public bool gameReset = false, gameOver = false;

    // Main Panel
    [SerializeField]
    private TextMeshProUGUI timer;
    [SerializeField]
    private TextMeshProUGUI currentLocation;
    [SerializeField]
    private TextMeshProUGUI percentCorrect;
    [SerializeField]
    private TextMeshProUGUI amountGuessed;

    // Game Over Panel
    [SerializeField]
    private GameObject gameOverObject;
    [SerializeField]
    private TextMeshProUGUI finalScoreText;
    [SerializeField]
    private TextMeshProUGUI finalPercentCorrectText;
    [SerializeField]
    private TextMeshProUGUI finalTimeText;

    // SFX
    [SerializeField]
    private AudioSource correctAnswerSFX;
    [SerializeField]
    private AudioSource wrongAnswerSFX;

    // Mouse UI
    [SerializeField]
    private GameObject mousePanel;


    private List<string> locations = new List<string>
    {
        "Swahili City-States", "Songhay", "Incan Empire", "Iroquois League",
        "Aztec Empire", "Mongolia", "Mughal Empire",
        "Mecca", "Indian Ocean", "South China Sea", "Beijing", "Florence",
        "Holy Roman Empire", "Spain", "Poland-Lithuania", "Ethiopia", "France",
        "Black Sea", "Muscovy", "West Indies", "Constantinople", "Ming Empire",
        "Ottoman Empire", "Japan", "Philippines"
    };

    private string[] guessedLocations;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ShuffleLocations();

        GiveNextLocation();
    }

    void Update()
    {
        if (!gameOver)
        keepTime();

        if(gameOver)
        GameOver();

        //Debug.Log(location);
    }

    void keepTime()
    {
        
        seconds += Time.deltaTime;

        int intSeconds = (int)seconds;

        int minutes = intSeconds / 60;
        if (intSeconds < 10 || minutes < 10)
        {
            if (intSeconds - (60 * minutes) < 10 && minutes < 10)
            {
                timer.text = "0" + minutes + ":0" + (intSeconds - (60*minutes));
            }
            else if (intSeconds - (60 * minutes) < 10)
            {
                timer.text = minutes + ":0" + (intSeconds - (60 * minutes));
            }
            else if (minutes < 10)
            {
                timer.text = "0" + minutes + ":" + (intSeconds - (60 * minutes));
            }
        }
        else
        {
            timer.text = minutes + ":" + (intSeconds - (60 * minutes));
        }
    }

    void ShuffleLocations()
    {
        for (int i = 0; i < locations.Count; i++)
        {
            string temp = locations[i];
            int randomIndex = UnityEngine.Random.Range(i, locations.Count);
            locations[i] = locations[randomIndex];
            locations[randomIndex] = temp;
        }
    }

    void GiveNextLocation()
    {
        if (currentIndex < locations.Count)
        {
            // Display or handle the current location
            currentLocation.text = locations[currentIndex];
            location = locations[currentIndex];

        }
        else
        {
            // Game Over
            Debug.Log("Game Over! Your score: " + correctGuesses);
            gameOver = true;
            
        }
    }

    public void PlayerGuessed(bool wasCorrect)
    {
        gameReset = false;

        if (wasCorrect)
        {
            if (currentGuesses < 2)
            {
                correctGuesses++;
                amountGuessed.text = correctGuesses + "/25";

            }

            correctAnswerSFX.Play();
            currentIndex++;
            currentGuesses = 0;
            GiveNextLocation();
            SetPercentage();

            Debug.Log("CORRECT!\n" + currentGuesses);
        }
        else
        {
            currentGuesses++;

            
            Debug.Log("WRONG!\n" + currentGuesses);

            wrongAnswerSFX.Play();
        }

        
    }

    // Shows the plyer the game over screen
    void GameOver()
    {
        gameOverObject.SetActive(gameOver);

        finalPercentCorrectText.text = percentCorrect.text;
        finalScoreText.text = amountGuessed.text;

        setFinalTime();
    }

    // Sets the players final time
    void setFinalTime()
    {
        string[] temp = timer.text.Split(':');

        for (int i = 0; i < temp.Length; i++)
        {
            finalTime[i] = int.Parse(temp[i]);
        }

        if (finalTime[0] < 10 || finalTime[1] < 10)
        {
            if (finalTime[0] < 10 && finalTime[1] < 10)
            {
                finalTimeText.text = "0" + finalTime[0] + ":0" + finalTime[1];
            }
            else if (finalTime[0] < 10)
            {
                finalTimeText.text = "0" + finalTime[0] + ":" + finalTime[1];
            }
            else if (finalTime[1] < 10)
            {
                finalTimeText.text = finalTime[0] + ":0" + finalTime[1];
            }
        }
        else
        {
            finalTimeText.text = finalTime[0] + ":" + finalTime[1];
        }
    }

    // Reset the game
    public void ResetGame()
    {
        gameReset = true;

        seconds = 0;
        currentIndex = 0;
        correctGuesses = 0;
        currentGuesses = 0;
        ShuffleLocations();
        GiveNextLocation();
        ResetLocations();

        gameOver = false;
        gameOverObject.SetActive(gameOver);

        mousePanel.SetActive(true);
    }

    public void ResetLocations()
    {
        LocationScript[] temp = FindObjectsOfType<LocationScript>();

        foreach (LocationScript script in temp)
        {
            script.ResetGame();
        }
    }

    void SetPercentage()
    {
        percentCorrect.text = (Math.Round(((double)correctGuesses / 25), 3)*100).ToString() + "%";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

