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

    bool canDeal = true;

    public int playedCardValue;

    [SerializeField] GameObject[] playerButtons;
    [SerializeField] GameObject[] cardValueButtons;

    Card hiddenCard;
    Card hiddenCardVisible;

    Player guardTarget;

    // Start is called before the first frame update
    void Start()
    {
        DisablePlayerButtons();
        deck = FindObjectOfType<Deck>();
        SetUpRound();
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

    void ChangeCurrentPlayer()
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
        DealCard();
        Card chosenCard = ChooseCard();
        Player chosenPlayer = ChoosePlayer();
        StartCoroutine(WaitAndPlayCard(chosenCard, chosenPlayer));
    }

    private Player ChoosePlayer()
    {
        List<Player> targets = AvailableTargets();
        Player chosenPlayer = null;
        if (targets.Count == 0)
        {
            chosenPlayer = null;
        } else if (targets.Count == 1)
        {
            chosenPlayer = targets[0];
        } else {
            int randomIndex = Random.Range(0, targets.Count - 1);
            chosenPlayer = targets[randomIndex];
        }
        return chosenPlayer;
    }

    private Card ChooseCard()
    {
        Card chosenCard = null;
        Card card1 = currentPlayer.GetCurrentCards()[0];
        Card card2 = currentPlayer.GetCurrentCards()[1];

        if (card1.GetValue() == 7 && card2.GetValue() == 5 || card1.GetValue() == 7 && card2.GetValue() == 6)
        {
            chosenCard = card1;
        }
        else if (card2.GetValue() == 7 && card1.GetValue() == 5 || card2.GetValue() == 7 && card1.GetValue() == 6)
        {
            chosenCard = card2;
        }
        else if (card1.GetValue() > card2.GetValue())
        {
            chosenCard = card2;
        } else 
        {
            chosenCard = card1;
        }
        return chosenCard;
    }

    IEnumerator WaitAndPlayCard(Card chosenCard, Player chosenPlayer)
    {
        yield return new WaitForSeconds(2);
        MoveCardToDiscard(chosenCard, currentPlayer);
        switch (chosenCard.tag)
            {
                case "Guard":
                    PlayGuardComputer(chosenPlayer);
                    break;
                case "Priest":
                    PriestTargetChosen(chosenPlayer);
                    break;
                case "Baron":
                    BaronTargetChosen(chosenPlayer);
                    break;
                case "Handmaid":
                    PlayHandmaid();
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
                case "Countess":
                    PlayCountess();
                    break;
                case "Princess":
                    PlayPrincess();
                    break;
                default:
                    break;
            }
        ChangeCurrentPlayer();
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

    private void DisplayPlayerButtons()
    {
        int numberOfActivePlayers = 0;
        int startIndex = playedCardValue == 5 ? 0 : 1;
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

    private void DisablePlayerButtons()
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
        if (currentPlayer.GetNumber() == 1){
            ChangeCurrentPlayer();
        }
    }

    public void TargetChosen(Player player){
        switch (playedCardValue)
        {
            case 1:
                GuardTargetChosen(player);
                break;
            case 2:
                PriestTargetChosen(player);
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
        if (currentPlayer.GetNumber() == 1){
            ChangeCurrentPlayer();
        }    
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
        if (currentPlayer.GetNumber() == 1){
            ChangeCurrentPlayer();
        }
    }

    private void PlayGuardComputer(Player player)
    {
        guardTarget = player;
        int cardValueGuess = Random.Range(2, 8);
        GuardTargetCardChosen(cardValueGuess);
    }

    public void GuardTargetChosen(Player player)
    {
        guardTarget = player;
        DisablePlayerButtons();
        DisplayTargetCardButtons();
    }

    public void GuardTargetCardChosen(int cardValue)
    {
        if (guardTarget != null && guardTarget.GetCurrentCard().GetValue() == cardValue)
        {
            MoveCardToDiscard(guardTarget.GetCurrentCard(), guardTarget);
            guardTarget.SetActive(false);
        }
        DisablePlayerButtons();
        canDeal = true;
        if (currentPlayer.GetNumber() == 1){
            ChangeCurrentPlayer();
        }
    }

    public void PriestTargetChosen(Player player)
    {
        if (player != null && currentPlayer.GetNumber() == 1)
        {
            player.GetCurrentCard().FlipCard();
            StartCoroutine(WaitAndFlip(player.GetCurrentCard()));
        } else 
        {
            canDeal = true;
            // ChangeCurrentPlayer();
        }
        
    }

    IEnumerator WaitAndFlip(Card card)
    {
        yield return new WaitForSeconds(3);
        card.FlipCard();
        DisablePlayerButtons();
        canDeal = true;
        ChangeCurrentPlayer();
    }

    public void PlayCard(Card card)
    {
        if (currentPlayer.GetCurrentCardsNumber() == 2 && currentPlayer.GetCurrentCards().Contains(card))
        {
            playedCardValue = card.GetValue();
            switch (card.tag)
            {
                case "Guard":
                    MoveCardToDiscard(card, currentPlayer);
                    PlayGuard();
                    break;
                case "Priest":
                    MoveCardToDiscard(card, currentPlayer);
                    PlayPriest();
                    break;
                case "Baron":
                    MoveCardToDiscard(card, currentPlayer);
                    PlayBaron();
                    break;
                case "Handmaid":
                    MoveCardToDiscard(card, currentPlayer);
                    PlayHandmaid();
                    break;
                case "Prince":
                    foreach (Card currentCard in currentPlayer.GetCurrentCards())
                    {
                        if (currentCard.GetValue() == 7)
                        {
                            return;
                        }
                    }
                    MoveCardToDiscard(card, currentPlayer);
                    PlayPrince();
                    break;
                case "King":
                    foreach (Card currentCard in currentPlayer.GetCurrentCards())
                        {
                            if (currentCard.GetValue() == 7)
                            {
                                return;
                            }
                        }
                    MoveCardToDiscard(card, currentPlayer);
                    PlayKing();
                    break;
                case "Countess":
                    MoveCardToDiscard(card, currentPlayer);
                    PlayCountess();
                    break;
                case "Princess":
                    MoveCardToDiscard(card, currentPlayer);
                    PlayPrincess();
                    break;
                default:
                    break;
            }
        }
    }

    public void PlayGuard()
    {
        DisplayPlayerButtons();
    }

    public void PlayPriest()
    {
        DisplayPlayerButtons();
    }

    public void PlayBaron()
    {
        DisplayPlayerButtons();
    }

    public void PlayHandmaid()
    {
        currentPlayer.SetInvincible(true);
        if (currentPlayer.GetNumber() == 1)
        {
            ChangeCurrentPlayer();
        }
    }

    public void PlayPrince()
    {
        DisplayPlayerButtons();
    }

    public void PlayKing()
    {
        DisplayPlayerButtons();
    }

    public void PlayCountess()
    {
        if (currentPlayer.GetNumber() == 1)
        {
            ChangeCurrentPlayer();
        }
    }

    public void PlayPrincess()
    {
        currentPlayer.SetActive(false);
        if (currentPlayer.GetNumber() == 1)
        {
            ChangeCurrentPlayer();
        }
    }

    public void MoveCardToDiscard(Card card, Player player)
    {
        card.GetComponent<SpriteRenderer>().sprite = card.GetFrontImage();
        player.RemoveCard(card);
        player.AddToPlayedCards(card);
        Vector3 discardPile = player.GetDiscardPile().transform.position;
        Vector3 discardPosition = new Vector3(0, 0, 0);
        if (player.GetPlayedCardsNumber() > 1)
        {
            if (player.GetNumber() == 1)
            {
                discardPosition = new Vector3(discardPile.x + (float)(player.GetPlayedCardsNumber() * 0.5 - 0.5), discardPile.y, discardPile.z - (player.GetPlayedCardsNumber() - 1));
            } else if (player.GetNumber() == 2)
            {
                discardPosition = new Vector3(discardPile.x, discardPile.y - (float)(player.GetPlayedCardsNumber() * 0.5 - 0.5), discardPile.z - (player.GetPlayedCardsNumber() - 1));            
            } else if (player.GetNumber() == 3)
            {
                discardPosition = new Vector3(discardPile.x - (float)(player.GetPlayedCardsNumber() * 0.5 - 0.5), discardPile.y, discardPile.z - (player.GetPlayedCardsNumber() - 1));            
            } else if (player.GetNumber() == 4)
            {
                discardPosition = new Vector3(discardPile.x, discardPile.y + (float)(player.GetPlayedCardsNumber() * 0.5 - 0.5), discardPile.z - (player.GetPlayedCardsNumber() - 1));            
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
        
        if (canDeal)
        {

            int previousPlayerNumber = (currentPlayerNumber == 0) ? 3 : currentPlayerNumber - 1;

            Player previousPlayer = players[previousPlayerNumber];

            if (currentPlayer.GetCurrentCardsNumber() < 2 && previousPlayer.GetCurrentCardsNumber() < 2)
            {
                deck.DealCard(currentPlayer);
            } else {

                return;
            }
        }
    }
}
