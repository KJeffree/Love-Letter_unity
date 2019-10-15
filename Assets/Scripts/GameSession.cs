using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{

    [SerializeField] Player[] players;

    Deck deck;

    public int gameLevel;

    bool gameInPlay = true;

    public TextMeshProUGUI gamePlayText;

    public Player currentPlayer;

    public Player lastWinner;

    public bool canDeal = true;

    public Card playedCard;

    [SerializeField] GameObject[] playerButtons;
    [SerializeField] GameObject[] cardValueButtons;

    Card hiddenCardVisible;

    Player guardTarget;

    Computer computer;

    SceneLoader sceneLoader;

    List<string> gameMoves = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        lastWinner = players[0];
        DisablePlayerButtons();
        sceneLoader = FindObjectOfType<SceneLoader>();
        gameLevel = sceneLoader.GetSelectedLevel();
        deck = FindObjectOfType<Deck>();
        SetUpRound();
        computer = FindObjectOfType<Computer>();
        gamePlayText.text = "";
    }

    public void SetLevel(int level)
    {
        gameLevel = level;
        Debug.Log("level changed");

    }

    private void SetUpRound()
    {
        // currentPlayerNumber = 0;
        canDeal = true;
        currentPlayer = lastWinner;
        deck.SetUpDeck();
        Card hiddenCard = deck.DealHiddenCard();
        hiddenCardVisible = Instantiate(hiddenCard, new Vector3(1.5f, 0, -1), Quaternion.Euler(0, 0, 0));

        foreach (Player player in players)
        {
            deck.DealCard(player);
        }
        gameInPlay = true;

        if (currentPlayer.GetNumber() > 1)
        {
            StartCoroutine(ComputerTurn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Player> activePlayers = new List<Player>();
        foreach (Player player in players)
        {
            if (player.GetActive())
            {
                activePlayers.Add(player);
            }
        }
        if (activePlayers.Count == 1 && gameInPlay)
        {
            StopAllCoroutines();
            GameOver();
        }
    }

    public void UpdateGamePlayText(string text)
    {
        gameMoves.Add(text);
        string gameText = "";
        for (int i = gameMoves.Count - 1; i >= 0; i--)
        {
            gameText += gameMoves[i] + "\n";
        }
        gamePlayText.text = gameText;
    }

    public void SetCanDeal(bool status)
    {
        canDeal = status;
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public Card GetHiddenCard()
    {
        return hiddenCardVisible;
    }

    public void ChangeCurrentPlayer()
    {
        if (deck.NumberOfCards() == 0)
        {
            GameOver();
            return;
        }

        int newIndex = currentPlayer.GetNumber();
        currentPlayer = currentPlayer.GetNumber() < 4 ? players[newIndex] : players[0];
        currentPlayer.SetInvincible(false);

        if (currentPlayer.GetActive() == false)
        {
            ChangeCurrentPlayer();
            return;
        }

        if (currentPlayer.GetNumber() != 1)
        {
            StartCoroutine(ComputerTurn());
        }
    }

    private void GameOver()
    {
        gameInPlay = false;
        canDeal = false;
        List<Player> activePlayers = new List<Player>();
        foreach (Player player in players)
        {
            if (player.GetActive())
            {
                activePlayers.Add(player);
                player.GetCurrentCard().ShowFrontImage();
            }
        }
        if (activePlayers.Count == 1)
        {
            activePlayers[0].AddPoint();
            lastWinner = activePlayers[0];
        } 
        else 
        {
            List<Player> playersWithHighestValueCard = new List<Player>();
            int highestCardValue = 0;
            foreach (Player player in activePlayers)
            {
                if (player.GetCurrentCard().GetValue() > highestCardValue)
                {
                    highestCardValue = player.GetCurrentCard().GetValue();
                    playersWithHighestValueCard.Clear();
                    playersWithHighestValueCard.Add(player);
                } else if (player.GetCurrentCard().GetValue() == highestCardValue)
                {
                    playersWithHighestValueCard.Add(player);
                }
            }

            if (playersWithHighestValueCard.Count == 1)
            {
                playersWithHighestValueCard[0].AddPoint();
                lastWinner = playersWithHighestValueCard[0];
            } 
            else 
            {
                List<Player> playersWithHighestValueDiscardPile = new List<Player>();
                int highestDiscardPileValue = 0;
                foreach (Player player in playersWithHighestValueCard)
                {
                    if (player.TotalValueOfPlayedCards() > highestDiscardPileValue)
                    {
                        highestDiscardPileValue = player.TotalValueOfPlayedCards();
                        playersWithHighestValueDiscardPile.Clear();
                        playersWithHighestValueDiscardPile.Add(player);
                    } else if (player.TotalValueOfPlayedCards() == highestDiscardPileValue)
                    {
                        playersWithHighestValueDiscardPile.Add(player);
                    }
                }
                if (playersWithHighestValueDiscardPile.Count == 1)
                {
                    playersWithHighestValueDiscardPile[0].AddPoint();
                    lastWinner = playersWithHighestValueDiscardPile[0];
                } else {
                    foreach (Player player in playersWithHighestValueDiscardPile)
                    {
                        player.AddPoint();
                        lastWinner = player;
                    }
                }
            }
        }

        foreach (Player player in players)
        {
            if (player.GetPoints() == 5)
            {
                sceneLoader.LoadGameOverScene();
                return;
            }
        }

        StartCoroutine(StartNewRound());
    }

    IEnumerator StartNewRound()
    {
        yield return new WaitForSeconds(5);
        foreach (Player player in players)
        {
            player.NewRound();
        }
        Card[] cards = FindObjectsOfType<Card>();
        foreach (Card card in cards)
        {
            Destroy(card.gameObject);
        }
        deck.ClearDeck();
        SetUpRound();
    }

    IEnumerator ComputerTurn()
    {
        yield return new WaitForSeconds(1);
        List<Player> availableTargets = AvailableTargets();
        computer.TakeTurn(currentPlayer, AvailableTargets());
    }

    private List<Player> AvailableTargets()
    {
        List<Player> availablePlayers = new List<Player>();
        foreach (Player player in players)
        {
            if (player.GetActive() && !player.GetInvincible() && player != currentPlayer)
            {
                availablePlayers.Add(player);
            }
        }
        return availablePlayers;
    }

    public void PlayCardComputer(Card chosenCard, Player chosenPlayer)
    {
        playedCard = chosenCard;
        MoveCardToDiscard(chosenCard, currentPlayer);
        switch (chosenCard.tag)
            {
                case "Guard":
                    playedCard.GetComponent<Guard>().PlayGuardComputer(chosenPlayer);
                    break;
                case "Priest":
                    playedCard.GetComponent<Priest>().PriestTargetChosen(chosenPlayer);
                    break;
                case "Baron":
                    playedCard.GetComponent<Baron>().BaronTargetChosen(chosenPlayer);
                    break;
                case "Prince":
                    if (!chosenPlayer)
                    {
                        chosenPlayer = currentPlayer;
                    }
                    playedCard.GetComponent<Prince>().PrinceTargetChosen(chosenPlayer);
                    break;
                case "King":
                    playedCard.GetComponent<King>().KingTargetChosen(chosenPlayer);
                    break;
                default:
                    break;
            }
    }

    private void DisplayPlayerButtons()
    {
        int numberOfActivePlayers = 0;
        int startIndex = playedCard.GetValue() == 5 ? 0 : 1;
        for (int i = startIndex; i < playerButtons.Length; i++)
        {
            if (players[i].GetActive() && !players[i].GetInvincible())
            {
                playerButtons[i].SetActive(true);
                numberOfActivePlayers++;
            }
        }
        canDeal = false;
        if (numberOfActivePlayers == 0)
        {
            ChangeCurrentPlayer();
            canDeal = true;
        }
    }

    private void DisplayTargetCardButtons()
    {
        foreach (GameObject button in cardValueButtons)
        {
            button.SetActive(true);
        }
    }

    public void DisablePlayerButtons()
    {
        foreach (GameObject button in playerButtons)
        {
            button.SetActive(false);
        }
        foreach (GameObject button in cardValueButtons)
        {
            button.SetActive(false);
        }
    }

    public void TargetChosen(Player player){
        switch (playedCard.GetValue())
        {
            case 1:
                GuardTargetChosen(player);
                break;
            case 2:
                playedCard.GetComponent<Priest>().PriestTargetChosen(player);
                break;
            case 3:
                playedCard.GetComponent<Baron>().BaronTargetChosen(player);
                break;
            case 5:
                playedCard.GetComponent<Prince>().PrinceTargetChosen(player);
                break;
            case 6:
                playedCard.GetComponent<King>().KingTargetChosen(player);
                break;
            default:
                break;
        }
    }


    public void GuardTargetCardChosen(int cardValue)
    {
        playedCard.GetComponent<Guard>().GuardTargetCardChosen(cardValue, guardTarget);
        canDeal = true;
    }

    public void GuardTargetChosen(Player player)
    {
        guardTarget = player;
        DisablePlayerButtons();
        DisplayTargetCardButtons();
    }

    public void PlayCard(Card card)
    {
        if (currentPlayer.GetCurrentCardsNumber() == 2 && currentPlayer.GetCurrentCards().Contains(card))
        {
            playedCard = card;
            switch (card.GetValue())
            {
                case int n when (n == 1 || n == 2 || n == 3):
                    MoveCardToDiscard(card, currentPlayer);
                    DisplayPlayerButtons();
                    break;
                case 4:
                    MoveCardToDiscard(card, currentPlayer);
                    UpdateGamePlayText("Handmaid played");
                    currentPlayer.SetInvincible(true);
                    ChangeCurrentPlayer();
                    break;
                case int n when (n == 5 || n == 6):
                    foreach (Card currentCard in currentPlayer.GetCurrentCards())
                    {
                        if (currentCard.GetValue() == 7)
                        {
                            return;
                        }
                    }
                    MoveCardToDiscard(card, currentPlayer);
                    DisplayPlayerButtons();
                    break;
                case 7:
                    MoveCardToDiscard(card, currentPlayer);
                    UpdateGamePlayText("Countess played");
                    ChangeCurrentPlayer();
                    break;
                case 8:
                    MoveCardToDiscard(card, currentPlayer);
                    currentPlayer.SetActive(false);
                    UpdateGamePlayText("Princess played");
                    ChangeCurrentPlayer();
                    break;
                default:
                    break;
            }
        }
    }

    public void MoveCardToDiscard(Card card, Player player)
    {
        card.ShowFrontImage();
        player.RemoveCard(card);
        player.AddToPlayedCards(card);
        Vector3 discardPile = player.GetDiscardPile().transform.position;
        float cardDisplacement = (float)(player.GetPlayedCardsNumber() * 0.5 - 0.5);
        int zPositionAlteration = player.GetPlayedCardsNumber() - 1;
        if (player.GetPlayedCardsNumber() > 1)
        {
            if (player.GetNumber() == 1)
            {
                card.PositionCard(discardPile.x + cardDisplacement, discardPile.y, discardPile.z - zPositionAlteration);
            } else if (player.GetNumber() == 2)
            {
                card.PositionCard(discardPile.x, discardPile.y - cardDisplacement, discardPile.z - zPositionAlteration);            
            } else if (player.GetNumber() == 3)
            {
                card.PositionCard(discardPile.x - cardDisplacement, discardPile.y, discardPile.z - zPositionAlteration);            
            } else if (player.GetNumber() == 4)
            {
                card.PositionCard(discardPile.x, discardPile.y + cardDisplacement, discardPile.z - zPositionAlteration);            
            }
        } else {
            card.PositionCard(discardPile.x, discardPile.y, discardPile.z);
        }
        player.PositionSingleCard();
        if (card.GetValue() == 8)
        {
            player.SetActive(false);
            if (player.GetCurrentCards().Count > 0)
            {
                MoveCardToDiscard(player.GetCurrentCard(), player);  
            }
        }
    }

    public void DealCard()
    {
        int previousPlayerNumber = (currentPlayer.GetNumber() == 1) ? 4 : currentPlayer.GetNumber() - 1;
        Player previousPlayer = players[previousPlayerNumber - 1];
        if (currentPlayer.GetCurrentCardsNumber() < 2 && previousPlayer.GetCurrentCardsNumber() < 2 && canDeal)
        {
            deck.DealCard(currentPlayer);
        } else {
            return;
        }
    }
}
