using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Base Sprites")]
    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorSprite;

    [Header("Panels")]
    public GameObject gameLobbyMenu;
    public GameObject endgameMenu;
    public GameObject gameMenu;
    public GameObject dlcStoreMenu;
    public GameObject waitingMenu;

    [Header("Game Lobby")]
    public TMP_InputField playerName;
    public TMP_InputField lobbyKey;
    public Button newLobbyBut;
    public Button joinLobbyBut;
    public TextMeshProUGUI updateText;

    [Header("Game Menu")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI statusText;
    public Image player1ChoiceImage;
    public Image player2ChoiceImage;
    public Button surrBut;
    public Button rockBut;
    public Button paperBut;
    public Button scissorsBut;

    [Header("End Game Menu")]
    public TextMeshProUGUI winnerText;

    [Header("DLC Store")]
    public TextMeshProUGUI statusText1;
    public TextMeshProUGUI statusText2;
    public TextMeshProUGUI statusText3;
    public Image selectedBkgImage;
    public TextMeshProUGUI background1Text;
    public TextMeshProUGUI background2Text;
    public TextMeshProUGUI background3Text;
    public Image background1;
    public Image background2;
    public Image background3;
    public Scrollbar progressBar;

    public int currPanel;
    public string winner;

    // Start is called before the first frame update
    void Start()
    {
        SwitchMenu(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActiveElements();
    }

    private void UpdateActiveElements()
    {
        switch (currPanel)
        {
            //Game Lobby
            case 0:
                if (playerName.text.Length == 0)
                {
                    newLobbyBut.interactable = false;
                }
                else
                {
                    newLobbyBut.interactable = true;
                }

                if (lobbyKey.text.Length == 0 || playerName.text.Length == 0)
                {
                    joinLobbyBut.interactable = false;
                }
                else
                {
                    joinLobbyBut.interactable = true;
                }
                break;
            //End Screen
            case 2:
                winnerText.text = "The Winner is " + winner;
                break;
            default:
                Debug.Log("Invalid menu id given - not updating elements");
                break;
        }
    }

    public void SwitchMenu(int menuId)
    {
        currPanel = menuId;
        switch (menuId)
        {
            //Game Lobby
            case 0:
                gameLobbyMenu.SetActive(true);
                endgameMenu.SetActive(false);
                gameMenu.SetActive(false);
                dlcStoreMenu.SetActive(false);
                waitingMenu.SetActive(false);
                break;
            //Game Menu
            case 1:
                gameLobbyMenu.SetActive(false);
                endgameMenu.SetActive(false);
                gameMenu.SetActive(true);
                dlcStoreMenu.SetActive(false);
                waitingMenu.SetActive(false);
                break;
            //End Game Menu
            case 2:
                gameLobbyMenu.SetActive(false);
                endgameMenu.SetActive(true);
                gameMenu.SetActive(false);
                dlcStoreMenu.SetActive(false);
                waitingMenu.SetActive(false);
                break;
            //DLC Store
            case 3:
                gameLobbyMenu.SetActive(false);
                endgameMenu.SetActive(false);
                gameMenu.SetActive(false);
                dlcStoreMenu.SetActive(true);
                waitingMenu.SetActive(false);
                break;
            //Waiting Menu
            case 4:
                gameLobbyMenu.SetActive(false);
                endgameMenu.SetActive(false);
                gameMenu.SetActive(false);
                dlcStoreMenu.SetActive(false);
                waitingMenu.SetActive(true);
                break;
            default:
                Debug.Log("Invalid menu id given - not switching panels");
                break;
        }
    }

    public void UpdateDLC(int bkgID, bool enable)
    {
        switch (bkgID)
        {
            //Game Menu
            case 1:
                if (enable == true)
                {
                    background1Text.text = "";
                    statusText1.text = "Purchased";
                    Color tempColor = background1.color;
                    tempColor.r = 1;
                    tempColor.g = 1;
                    tempColor.b = 1;
                    tempColor.a = 1;
                    background1.color = tempColor;
                }
                else
                {
                    background1Text.text = "?";
                    statusText1.text = "Not Owned Yet";
                    Color tempColor = background1.color;
                    tempColor.r = 63;
                    tempColor.g = 63;
                    tempColor.b = 63;
                    tempColor.a = 255;
                    background1.color = tempColor;
                }
                break;
            //End Game Menu
            case 2:
                if (enable == true)
                {
                    background2Text.text = "";
                    statusText2.text = "Purchased";
                    Color tempColor = background2.color;
                    tempColor.r = 1;
                    tempColor.g = 1;
                    tempColor.b = 1;
                    tempColor.a = 1;
                    background2.color = tempColor;
                }
                else
                {
                    background2Text.text = "?";
                    statusText2.text = "Not Owned Yet";
                    Color tempColor = background2.color;
                    tempColor.r = 63;
                    tempColor.g = 63;
                    tempColor.b = 63;
                    tempColor.a = 255;
                    background2.color = tempColor;
                }
                break;
            //DLC Store
            case 3:
                if (enable == true)
                {
                    background3Text.text = "";
                    statusText3.text = "Purchased";
                    Color tempColor = background3.color;
                    tempColor.r = 1;
                    tempColor.g = 1;
                    tempColor.b = 1;
                    tempColor.a = 1;
                    background3.color = tempColor;
                }
                else
                {
                    background3Text.text = "?";
                    statusText3.text = "Not Owned Yet";
                    Color tempColor = background3.color;
                    tempColor.r = 63;
                    tempColor.g = 63;
                    tempColor.b = 63;
                    tempColor.a = 255;
                    background3.color = tempColor;
                }
                break;
            default:
                Debug.Log("Invalid background id given - not updating elements");
                break;
        }
    }

    public void UpdateGameSelection(bool player, int type, bool enable)
    {
        Color tempColor = new Color();
        if (enable == true)
        {
            if (player == false)
            {
                switch (type)
                {
                    case 0:
                        player1ChoiceImage.sprite = rockSprite;
                        tempColor = player1ChoiceImage.color;
                        tempColor.r = 1;
                        tempColor.g = 1;
                        tempColor.b = 1;
                        tempColor.a = 1;
                        player1ChoiceImage.color = tempColor;
                        break;
                    case 1:
                        player1ChoiceImage.sprite = paperSprite;
                        tempColor = player1ChoiceImage.color;
                        tempColor.r = 1;
                        tempColor.g = 1;
                        tempColor.b = 1;
                        tempColor.a = 1;
                        player1ChoiceImage.color = tempColor;
                        break;
                    case 2:
                        player1ChoiceImage.sprite = scissorSprite;
                        tempColor = player1ChoiceImage.color;
                        tempColor.r = 1;
                        tempColor.g = 1;
                        tempColor.b = 1;
                        tempColor.a = 1;
                        player1ChoiceImage.color = tempColor;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case 0:
                        player2ChoiceImage.sprite = rockSprite;
                        tempColor = player2ChoiceImage.color;
                        tempColor.r = 1;
                        tempColor.g = 1;
                        tempColor.b = 1;
                        tempColor.a = 1;
                        player2ChoiceImage.color = tempColor;
                        break;
                    case 1:
                        player2ChoiceImage.sprite = paperSprite;
                        tempColor = player2ChoiceImage.color;
                        tempColor.r = 1;
                        tempColor.g = 1;
                        tempColor.b = 1;
                        tempColor.a = 1;
                        player2ChoiceImage.color = tempColor;
                        break;
                    case 2:
                        player2ChoiceImage.sprite = scissorSprite;
                        tempColor = player2ChoiceImage.color;
                        tempColor.r = 1;
                        tempColor.g = 1;
                        tempColor.b = 1;
                        tempColor.a = 1;
                        player2ChoiceImage.color = tempColor;
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            if (player == false)
            {
                player1ChoiceImage.sprite = null;
                tempColor = player1ChoiceImage.color;
                tempColor.r = 137;
                tempColor.g = 164;
                tempColor.b = 32;
                tempColor.a = 0;
                player1ChoiceImage.color = tempColor;
            }
            else
            {
                player2ChoiceImage.sprite = null;
                tempColor = player2ChoiceImage.color;
                tempColor.r = 137;
                tempColor.g = 164;
                tempColor.b = 32;
                tempColor.a = 0;
                player2ChoiceImage.color = tempColor;
            }
        }
    }

    //Game Menu
    public void UpdateButtons(bool enable)
    {
        if (enable == true)
        {
            surrBut.interactable = true;
            rockBut.interactable = true;
            paperBut.interactable = true;
            scissorsBut.interactable = true;
        }
        else
        {
            surrBut.interactable = false;
            rockBut.interactable = false;
            paperBut.interactable = false;
            scissorsBut.interactable = false;
        }
    }
}
