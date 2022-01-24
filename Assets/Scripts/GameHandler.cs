using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public UIManager ui;
    public DataHandler dh;
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