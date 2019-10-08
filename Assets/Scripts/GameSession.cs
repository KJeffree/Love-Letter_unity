using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    [SerializeField] Player[] players;

    Deck deck;

    bool gameInPlay = true;

    int currentPlayerNumber = 0;

    public Player currentPlayer;

    public bool canDeal = true;

    public Card playedCard;

    [SerializeField] GameObject[] playerButtons;
    [SerializeField] GameObject[] cardValueButtons;

    Card hiddenCard;
    Card hiddenCardVisible;

    Player guardTarget;

    Computer computer;

    // Start is called before the first frame update
    void Start()
    {
        DisablePlayerButtons();
        deck = FindObjectOfType<Deck>();
        SetUpRound();
        computer = FindObjectOfType<Computer>();
    }

    private void SetUpRound()
    {
        currentPlayerNumber = 0;
        deck.SetUpDeck();
        hiddenCard = deck.DealHiddenCard();
        hiddenCardVisible = Instantiate(hiddenCard, new Vector3(1.5f, 0, -1), Quaternion.Euler(0, 0, 0));

        foreach (Player player in players)
        {
            deck.DealCard(player);
        }
        currentPlayer = players[currentPlayerNumber];
        gameInPlay = true;
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

    public void SetCanDeal(bool status)
    {
        canDeal = status;
    }

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }
    public void ChangeCurrentPlayer()
    {
        if (deck.NumberOfCards() == 0)
        {
            GameOver();
            return;
        }

        if (currentPlayerNumber < 3)
        {
            currentPlayerNumber++;
        } else
        {
            currentPlayerNumber = 0;
        }
        currentPlayer = players[currentPlayerNumber];

        currentPlayer.SetInvincible(false);
        
        if (currentPlayer.GetActive() == false)
        {
            ChangeCurrentPlayer();
            return;
        }

        if (currentPlayerNumber != 0)
        {
            StartCoroutine(ComputerTurn());
        }
    }

    private void GameOver()
    {
        gameInPlay = false;
        List<Player> activePlayers = new List<Player>();
        foreach (Player player in players)
        {
            if (player.GetActive())
            {
                activePlayers.Add(player);
            }
        }
        if (activePlayers.Count == 1)
        {
            activePlayers[0].AddPoint();
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
                } else {
                    foreach (Player player in playersWithHighestValueDiscardPile)
                    {
                        player.AddPoint();
                    }
                }
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
                    PlayGuardComputer(chosenPlayer);
                    break;
                case "Priest":
                    playedCard.GetComponent<Priest>().PriestTargetChosen(chosenPlayer);
                    break;
                case "Baron":
                    BaronTargetChosen(chosenPlayer);
                    break;
                case "Prince":
                    if (!chosenPlayer)
                    {
                        chosenPlayer = currentPlayer;
                    }
                    PrinceTargetChosen(chosenPlayer);
                    break;
                case "King":
                    KingTargetChosen(chosenPlayer);
                    break;
                default:
                    break;
            }
        // ChangeCurrentPlayer();
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

    public void BaronTargetChosen(Player player)
    {
        if (player != null)
        {
            if (player.GetCurrentCardValue() > currentPlayer.GetCurrentCardValue())
            {
                MoveCardToDiscard(currentPlayer.currentCards[0], currentPlayer);
                currentPlayer.SetActive(false);
            } else if (player.GetCurrentCardValue() < currentPlayer.GetCurrentCardValue())
            {
                MoveCardToDiscard(player.currentCards[0], player);
                player.SetActive(false);
            }
        }
        DisablePlayerButtons();
        canDeal = true;
        ChangeCurrentPlayer();
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
                BaronTargetChosen(player);
                break;
            case 5:
                PrinceTargetChosen(player);
                break;
            case 6:
                KingTargetChosen(player);
                break;
            default:
                break;
        }
    }

    public void PrinceTargetChosen(Player player)
    {
        if (player.GetCurrentCard().GetValue() == 8)
        {
            MoveCardToDiscard(player.GetCurrentCard(), player);
        } else {
            MoveCardToDiscard(player.GetCurrentCard(), player);
            if (deck.NumberOfCards() > 0)
            {
                deck.DealCard(player);
            } else
            {
                player.AddCard(hiddenCard);
                Destroy(hiddenCardVisible.gameObject);
            }
        }
        DisablePlayerButtons();
        canDeal = true;
        ChangeCurrentPlayer();
    }

    public void KingTargetChosen(Player player)
    {
        if (player != null)
        {
            Card currentPlayerCard = currentPlayer.GetCurrentCard();
            Card targetPlayerCard = player.GetCurrentCard();
            currentPlayer.SwapCard(targetPlayerCard);
            player.SwapCard(currentPlayerCard);
        }
        DisablePlayerButtons();
        canDeal = true;      
        ChangeCurrentPlayer();
    }

    public void GuardTargetCardChosen(int cardValue)
    {
        playedCard.GetComponent<Guard>().GuardTargetCardChosen(cardValue, guardTarget);
        canDeal = true;
    }

    private void PlayGuardComputer(Player player)
    {
        guardTarget = player;
        int cardValueGuess = Random.Range(2, 8);
        playedCard.GetComponent<Guard>().GuardTargetCardChosen(cardValueGuess, guardTarget);
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
                    ChangeCurrentPlayer();
                    break;
                case 8:
                    MoveCardToDiscard(card, currentPlayer);
                    currentPlayer.SetActive(false);
                    ChangeCurrentPlayer();
                    break;
                default:
                    break;
            }
        }
    }

    public void MoveCardToDiscard(Card card, Player player)
    {
        card.GetComponent<SpriteRenderer>().sprite = card.GetFrontImage();
        player.RemoveCard(card);
        player.AddToPlayedCards(card);
        Vector3 discardPile = player.GetDiscardPile().transform.position;
        Vector3 discardPosition = new Vector3(0, 0, 0);
        float cardDisplacement = (float)(player.GetPlayedCardsNumber() * 0.5 - 0.5);
        int zPositionAlteration = player.GetPlayedCardsNumber() - 1;
        if (player.GetPlayedCardsNumber() > 1)
        {
            if (player.GetNumber() == 1)
            {
                discardPosition = new Vector3(discardPile.x + cardDisplacement, discardPile.y, discardPile.z - zPositionAlteration);
            } else if (player.GetNumber() == 2)
            {
                discardPosition = new Vector3(discardPile.x, discardPile.y - cardDisplacement, discardPile.z - zPositionAlteration);            
            } else if (player.GetNumber() == 3)
            {
                discardPosition = new Vector3(discardPile.x - cardDisplacement, discardPile.y, discardPile.z - zPositionAlteration);            
            } else if (player.GetNumber() == 4)
            {
                discardPosition = new Vector3(discardPile.x, discardPile.y + cardDisplacement, discardPile.z - zPositionAlteration);            
            }
        } else {
            discardPosition = player.GetDiscardPile().transform.position;
        }
        card.transform.position = discardPosition;
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
        int previousPlayerNumber = (currentPlayerNumber == 0) ? 3 : currentPlayerNumber - 1;
        Player previousPlayer = players[previousPlayerNumber];
        if (currentPlayer.GetCurrentCardsNumber() < 2 && previousPlayer.GetCurrentCardsNumber() < 2 && canDeal)
        {
            deck.DealCard(currentPlayer);
        } else {
            return;
        }
    }
}
