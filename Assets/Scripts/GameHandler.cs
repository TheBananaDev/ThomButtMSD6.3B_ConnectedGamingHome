using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public UIManager ui;
    public bool bothConnected;

    private int gameState;
    private int player1Score;
    private int player2Score;
    private int currRound;
    private int player1Choice;
    private int player2Choice;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIManager>();
        gameState = 0;
        //bothConnected = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameState);
        ui.timerText.text = "Time Left: "+Mathf.Round(timer);
        //Ongoing round
        if (timer > 0)
        {
            timer -= Time.deltaTime;
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
        player1Choice = 0;
        player2Choice = 0;
        player1Score = 0;
        player2Score = 0;
        currRound = 0;
        timer = 10f;
        ui.UpdateGameSelection(false, 0, false);
        ui.UpdateGameSelection(true, 0, false);
        ui.statusText.text = "Pick your move!";
    }

    private void StartNewRound()
    {
        ui.UpdateButtons(true);
        gameState = 1;
        player1Choice = 0;
        player2Choice = 0;
        currRound = currRound + 1;
        timer = 10f;
        ui.UpdateGameSelection(false, 0, false);
        ui.UpdateGameSelection(true, 0, false);
        ui.statusText.text = "Pick your move!";
    }

    private void StartGracePeriod()
    {
        ui.UpdateButtons(false);
        gameState = 2;
        timer = 5f;
        CalculateRoundWinner();
    }

    public void StartLobby()
    {
        gameState = -1;
    }

    public void Forfeit()
    {
        player2Score = player2Score + 2;
        currRound = 5;
        CalculateRoundWinner();
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
            gameState = 0;
            //Both players win
            if (player1Score == player2Score)
            {
                ui.winner = "Both";
            }
            //Player 1 wins
            else if (player1Score > player2Score)
            {
                ui.winner = "Player 1";
            }
            //Player 2 wins
            else if (player1Score < player2Score)
            {
                ui.winner = "Player 2";
            }
            ui.SwitchMenu(2);
        }
    }

    private void AddScore()
    {
        //No input
        if (player1Choice == 0 || player2Choice == 0)
        {
            if (player1Choice == 0 && player2Choice == 0)
            {
                ui.statusText.text = "Draw!";
            }
            else if (player1Choice == 0)
            {
                ui.statusText.text = "You lose!";
                player2Score = player2Score + 1;
            }
            else if (player2Choice == 0)
            {
                ui.statusText.text = "You win!";
                player1Score = player1Score + 1;
            }
        }

        switch (player1Choice)
        {
            //Rock
            case 1:
                switch (player2Choice)
                {
                    //Rock - Tie
                    case 1:
                        ui.statusText.text = "Draw!";
                        player1Score = player1Score + 1;
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
                        player1Score = player1Score + 1;
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
                        player1Score = player1Score + 1;
                        break;
                    //Paper - Tie
                    case 2:
                        ui.statusText.text = "Draw!";
                        player1Score = player1Score + 1;
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
                        player1Score = player1Score + 1;
                        break;
                    //Scissors - Tie
                    case 3:
                        ui.statusText.text = "Draw!";
                        player1Score = player1Score + 1;
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
        player1Choice = choice;
        ui.UpdateGameSelection(false, choice, true);
        switch (choice)
        {
            case 0:
                ui.statusText.text = "Rock";
                break;
            case 1:
                ui.statusText.text = "Paper";
                break;
            case 2:
                ui.statusText.text = "Scissors";
                break;
            default:
                break;
        }
    }
}
