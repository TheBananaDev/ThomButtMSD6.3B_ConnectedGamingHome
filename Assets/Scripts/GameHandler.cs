using Firebase.Extensions;
using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public UIManager ui;
    public DataHandler dh;
    public DLCDownloader dl;

    public bool bothConnected;
    public bool playerType;
    public int gameState;

    private int localPlayerScore;
    private int player2Score;

    private int currRound;
    private int localPlayerChoice;
    public int player2Choice;
    private float timer;
    private float totalTimer;
    private float totalmoves;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIManager>();
        dh = FindObjectOfType<DataHandler>();
        dl = FindObjectOfType<DLCDownloader>();
        gameState = 0;
        bothConnected = false;
    }

    // Update is called once per frame
    void Update()
    {
        ui.timerText.text = "Time Left: "+Mathf.Round(timer);
        //Ongoing round
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            totalTimer += Time.deltaTime;
        }
        //End of round - start grace period
        if (timer <= 0 && gameState == 1)
        {
            StartGracePeriod();
        }
        //End of grace period - start round
        else if (timer <= 0 && gameState == 2)
        {
            StartNewRound();
        }
        //Waiting
        if (gameState == -1)
        {
            if(bothConnected == true)
            {
                StartNewGame();
                ui.SwitchMenu(1);
            }
        }
    }

    private void StartNewGame()
    {
        gameState = 1;
        localPlayerChoice = 0;
        player2Choice = 0;
        localPlayerScore = 0;
        player2Score = 0;
        currRound = 0;
        timer = 10f;
        ui.UpdateGameSelection(false, 0, false);
        ui.UpdateGameSelection(true, 0, false);
        ui.statusText.text = "Pick your move!";
        totalmoves = 0;
        totalTimer = 0;

        if(dl.bkg1Purchased == true)
        {
            int randMax = 1;
            if (dl.bkg2Purchased == true)
            {
                randMax += 1;
            }
            if (dl.bkg3Purchased == true)
            {
                randMax += 1;
            }
            int rand = Random.Range(1, randMax);

            FirebaseStorage storage = FirebaseStorage.DefaultInstance;
            StorageReference bkg1Ref = storage.GetReferenceFromUrl("gs://connectedgamingassignment.appspot.com/Background/background1.jpg");
            StorageReference bkg2Ref = storage.GetReferenceFromUrl("gs://connectedgamingassignment.appspot.com/Background/background2.jpg");
            StorageReference bkg3Ref = storage.GetReferenceFromUrl("gs://connectedgamingassignment.appspot.com/Background/background3.png");
            const long maxAllowedSize = 1 * 1024 * 1024;
            switch (rand)
            {
                case 1:
                    bkg1Ref.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task1 => {
                        if (task1.IsFaulted || task1.IsCanceled)
                        {
                            Debug.LogException(task1.Exception);
                            // Uh-oh, an error occurred!
                        }
                        else
                        {
                            //Turns the image into a byte stream so it can be processed
                            byte[] fileContents = task1.Result;
                            Debug.Log("Finished downloading!");

                            //Converts the byte stream to a Texture2D component and displays it
                            Texture2D texture = new Texture2D(1920, 1080);
                            texture.LoadImage(fileContents);
                            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1920f, 1080f), new Vector2(0, 0), 100f);
                            ui.gameMenu.GetComponent<Image>().sprite = sprite;
                        }
                    });
                    break;
                case 2:
                    bkg2Ref.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task2 => {
                        if (task2.IsFaulted || task2.IsCanceled)
                        {
                            Debug.LogException(task2.Exception);
                            // Uh-oh, an error occurred!
                        }
                        else
                        {
                            //Turns the image into a byte stream so it can be processed
                            byte[] fileContents = task2.Result;
                            Debug.Log("Finished downloading!");

                            //Converts the byte stream to a Texture2D component and displays it
                            Texture2D texture = new Texture2D(1920, 1080);
                            texture.LoadImage(fileContents);
                            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1920f, 1080f), new Vector2(0, 0), 100f);
                            ui.gameMenu.GetComponent<Image>().sprite = sprite;
                        }
                    });
                    break;
                case 3:
                    bkg3Ref.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread(task3 => {
                        if (task3.IsFaulted || task3.IsCanceled)
                        {
                            Debug.LogException(task3.Exception);
                            // Uh-oh, an error occurred!
                        }
                        else
                        {
                            //Turns the image into a byte stream so it can be processed
                            byte[] fileContents = task3.Result;
                            Debug.Log("Finished downloading!");

                            //Converts the byte stream to a Texture2D component and displays it
                            Texture2D texture = new Texture2D(1920, 1080);
                            texture.LoadImage(fileContents);
                            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1920f, 1080f), new Vector2(0, 0), 100f);
                            ui.gameMenu.GetComponent<Image>().sprite = sprite;
                        }
                    });
                    break;
            }
        }
    }

    private void StartNewRound()
    {
        dh.UpdateChoice(0, false);
        dh.UpdateChoice(0, true);
        ui.UpdateButtons(true);
        gameState = 1;
        localPlayerChoice = 0;
        player2Choice = 0;
        currRound = currRound + 1;
        timer = 10f;
        ui.UpdateGameSelection(false, 0, false);
        ui.UpdateGameSelection(true, 0, false);
        ui.statusText.text = "Pick your move!";
    }

    private void StartGracePeriod()
    {
        ui.UpdateGameSelection(true, player2Choice, true);
        ui.UpdateButtons(false);
        gameState = 2;
        timer = 5f;
        CalculateRoundWinner();
    }

    public void StartLobby()
    {
        gameState = -1;
        dh.NewLobby();
    }

    public void Forfeit()
    {
        if(playerType == false)
        {
            player2Score = player2Score + 2;
        }
        else
        {
            localPlayerScore = localPlayerScore + 2;
        }
        currRound = 5;
        CalculateRoundWinner();
    }

    public void EmergencyQuit()
    {
        timer = 0;
        gameState = 0;
        //Both players win
        if (localPlayerScore == player2Score)
        {
            ui.winner = "Both";
        }
        //Player 1 wins
        else if (localPlayerScore > player2Score)
        {
            ui.winner = "Player 1";
        }
        //Player 2 wins
        else if (localPlayerScore < player2Score)
        {
            ui.winner = "Player 2";
        }
        ui.SwitchMenu(2);
    }

    private void CalculateRoundWinner()
    {
        //Round finished - go to next round
        if (currRound < 4)
        {
            AddScore();
        }
        //Round finished - finish game
        else
        {
            AddScore();
            timer = 0;
            gameState = 0;
            //Both players win
            if (localPlayerScore == player2Score)
            {
                ui.winner = "Both";
            }
            //Player 1 wins
            else if (localPlayerScore > player2Score)
            {
                ui.winner = "Player 1";
            }
            //Player 2 wins
            else if (localPlayerScore < player2Score)
            {
                ui.winner = "Player 2";
            }
            ui.SwitchMenu(2);
        }
    }

    private void AddScore()
    {
        //No input
        if (localPlayerChoice == 0 || player2Choice == 0)
        {
            if (localPlayerChoice == 0 && player2Choice == 0)
            {
                ui.statusText.text = "Draw!";
            }
            else if (localPlayerChoice == 0)
            {
                ui.statusText.text = "You lose!";
                player2Score = player2Score + 1;
            }
            else if (player2Choice == 0)
            {
                ui.statusText.text = "You win!";
                localPlayerScore = localPlayerScore + 1;
            }
        }

        switch (localPlayerChoice)
        {
            //Rock
            case 1:
                switch (player2Choice)
                {
                    //Rock - Tie
                    case 1:
                        ui.statusText.text = "Draw!";
                        localPlayerScore = localPlayerScore + 1;
                        player2Score = player2Score + 1;
                        break;
                    //Paper - Player 2
                    case 2:
                        ui.statusText.text = "You lose!";
                        player2Score = player2Score + 1;
                        break;
                    //Scissors - Player 1
                    case 3:
                        ui.statusText.text = "You win!";
                        localPlayerScore = localPlayerScore + 1;
                        break;
                    //Invalid
                    default:
                        break;
                }
                break;
            //Paper
            case 2:
                switch (player2Choice)
                {
                    //Rock - Player 1
                    case 1:
                        ui.statusText.text = "You win!";
                        localPlayerScore = localPlayerScore + 1;
                        break;
                    //Paper - Tie
                    case 2:
                        ui.statusText.text = "Draw!";
                        localPlayerScore = localPlayerScore + 1;
                        player2Score = player2Score + 1;
                        break;
                    //Scissors - Player 2
                    case 3:
                        ui.statusText.text = "You lose!";
                        player2Score = player2Score + 1;
                        break;
                    //Invalid
                    default:
                        break;
                }
                break;
            //Scissors
            case 3:
                switch (player2Choice)
                {
                    //Rock - Player 2
                    case 1:
                        ui.statusText.text = "You lose!";
                        player2Score = player2Score + 1;
                        break;
                    //Paper - Player 1
                    case 2:
                        ui.statusText.text = "You win!";
                        localPlayerScore = localPlayerScore + 1;
                        break;
                    //Scissors - Tie
                    case 3:
                        ui.statusText.text = "Draw!";
                        localPlayerScore = localPlayerScore + 1;
                        player2Score = player2Score + 1;
                        break;
                    //Invalid
                    default:
                        break;
                }
                break;
            //Invalid
            default:
                break;
        }
    }

    public void GetPlayerChoice(int choice)
    {
        localPlayerChoice = choice;
        if (playerType == false)
        {
            
            dh.UpdateChoice(localPlayerChoice, false);
        }
        else
        {
            dh.UpdateChoice(localPlayerChoice, true);
        }
        
        ui.UpdateGameSelection(false, choice, true);
        switch (choice)
        {
            case 1:
                ui.statusText.text = "Rock";
                break;
            case 2:
                ui.statusText.text = "Paper";
                break;
            case 3:
                ui.statusText.text = "Scissors";
                break;
            default:
                break;
        }
        totalmoves = totalmoves + 1;
    }
}