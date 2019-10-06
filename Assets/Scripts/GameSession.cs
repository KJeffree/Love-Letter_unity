using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    [SerializeField] Player[] players;

    Deck deck;

    int currentPlayerNumber = 0;

    Player currentPlayer;

    bool canDeal = true;

    [SerializeField] GameObject[] baronButtons;
    [SerializeField] GameObject[] guardButtons;
    [SerializeField] GameObject[] princeButtons;
    [SerializeField] GameObject[] kingButtons;
    [SerializeField] GameObject[] priestButtons;
    [SerializeField] GameObject[] cardValueButtons;

    Card hiddenCard;
    Card hiddenCardVisible;

    Player guardTarget;

    // Start is called before the first frame update
    void Start()
    {
        DisablePlayerButtons();
        deck = FindObjectOfType<Deck>();

        hiddenCard = deck.DealHiddenCard();
        hiddenCardVisible = Instantiate(hiddenCard, new Vector3(1.5f, 0, -1), Quaternion.Euler(0, 0, 0));

        foreach (Player player in players)
        {
            deck.DealCard(player);
        }
    }


    // Update is called once per frame
    void Update()
    {
        currentPlayer = players[currentPlayerNumber];
    }

    void ChangeCurrentPlayer()
    {
        if (currentPlayerNumber < 3)
        {
            currentPlayerNumber++;
        } else
        {
            currentPlayerNumber = 0;
        }
        players[currentPlayerNumber].SetInvincible(false);
        
        if (players[currentPlayerNumber].GetActive() == false)
        {
            ChangeCurrentPlayer();
        }

        currentPlayer = players[currentPlayerNumber];

        if (currentPlayerNumber != 0)
        {
            ComputerTurn();
        }
    }

    private void ComputerTurn()
    {
        DealCard();
        Card chosenCard = ChooseCard();
        Player chosenPlayer = ChoosePlayer();
        Debug.Log(chosenPlayer);
        StartCoroutine(WaitAndPlayCard(chosenCard, chosenPlayer));


        // if 1, 2, 3, or 6 choose a random player other than self.
        // if 5 choose random player including self, unless other card is princess.
        // if 5 and only available choice, chose self.
        // 
    }

    private Player ChoosePlayer()
    {
        List<Player> targets = AvailableTargets();
        Player chosenPlayer = null;
        if (targets.Count == 0)
        {

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
                    Debug.Log("Guard chosen on " + chosenPlayer);
                    break;
                case "Priest":
                    PriestTargetChosen(chosenPlayer);
                    Debug.Log("Priest chosen on " + chosenPlayer);
                    break;
                case "Baron":
                    BaronTargetChosen(chosenPlayer);
                    Debug.Log("Baron chosen on " + chosenPlayer);
                    break;
                case "Handmaid":
                    PlayHandmaid();
                    Debug.Log("Handmaid chosen");
                    break;
                case "Prince":
                    PrinceTargetChosen(chosenPlayer);
                    Debug.Log("Prince chosen on " + chosenPlayer);
                    break;
                case "King":
                    KingTargetChosen(chosenPlayer);
                    Debug.Log("King chosen on " + chosenPlayer);
                    break;
                case "Countess":
                    PlayCountess();
                    Debug.Log("Countess chosen");
                    break;
                case "Princess":
                    PlayPrincess();
                    Debug.Log("Princess chosen");
                    break;
                default:
                    break;
            }
    }

    private List<Player> AvailableTargets()
    {
        List<Player> availablePlayers = new List<Player>();
        foreach (Player player in players)
        {
            if (player.GetActive() && !player.GetInvincible() && player != currentPlayer)
            {
                availablePlayers.Add(player);
                Debug.Log(player + " added");
            }
        }
        return availablePlayers;
    }

    private void DisplayPlayerButtonsPrince()
    {
        for (int i = 0; i < princeButtons.Length; i++)
        {
            if (players[i].GetActive() && !players[i].GetInvincible())
            {
                princeButtons[i].SetActive(true);
            }
        }
        canDeal = false;
    }

    private void DisplayPlayerButtonsBaron()
    {
        for (int i = 0; i < baronButtons.Length; i++)
        {
            if (players[i+1].GetActive() && !players[i+1].GetInvincible())
            {
                baronButtons[i].SetActive(true);
            }
        }
        canDeal = false;
    }

    private void DisplayPlayerButtonsKing()
    {
        for (int i = 0; i < kingButtons.Length; i++)
        {
            if (players[i+1].GetActive() && !players[i+1].GetInvincible())
            {
                kingButtons[i].SetActive(true);
            }
        }
        canDeal = false;
    }

    private void DisplayTargetCardButtons()
    {
        foreach (GameObject button in cardValueButtons)
        {
            button.SetActive(true);
        }
    }

    private void DisplayPlayerButtonsGuard()
    {
        for (int i = 0; i < guardButtons.Length; i++)
        {
            if (players[i+1].GetActive() && !players[i+1].GetInvincible())
            {
                guardButtons[i].SetActive(true);
            }
        }
        canDeal = false;
    }

    private void DisplayPlayerButtonsPriest()
    {
        for (int i = 0; i < priestButtons.Length; i++)
        {
            if (players[i+1].GetActive() && !players[i+1].GetInvincible())
            {
                priestButtons[i].SetActive(true);
            }
        }
        canDeal = false;
    }

    private void DisablePlayerButtons()
    {
        foreach (GameObject button in baronButtons)
        {
            button.SetActive(false);
        }
        foreach (GameObject button in guardButtons)
        {
            button.SetActive(false);
        }
        foreach (GameObject button in princeButtons)
        {
            button.SetActive(false);
        }
        foreach (GameObject button in kingButtons)
        {
            button.SetActive(false);
        }
        foreach (GameObject button in priestButtons)
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
        if (player.GetCurrentCardValue() > currentPlayer.GetCurrentCardValue())
        {
            MoveCardToDiscard(currentPlayer.currentCards[0], currentPlayer);
            currentPlayer.SetActive(false);
        } else if (player.GetCurrentCardValue() < currentPlayer.GetCurrentCardValue())
        {
            MoveCardToDiscard(player.currentCards[0], player);
            player.SetActive(false);
        }
        canDeal = true;
        ChangeCurrentPlayer();
        DisablePlayerButtons();
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
        Card currentPlayerCard = currentPlayer.GetCurrentCard();
        Card targetPlayerCard = player.GetCurrentCard();
        currentPlayer.SwapCard(targetPlayerCard);
        player.SwapCard(currentPlayerCard);
        DisablePlayerButtons();
        canDeal = true;      
        ChangeCurrentPlayer(); 
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
        if (guardTarget.GetCurrentCard().GetValue() == cardValue)
        {
            MoveCardToDiscard(guardTarget.GetCurrentCard(), guardTarget);
            guardTarget.SetActive(false);
        }
        DisablePlayerButtons();
        canDeal = true;
        ChangeCurrentPlayer();
    }

    public void PriestTargetChosen(Player player)
    {
        if (currentPlayer.GetNumber() == 1)
        {
            player.GetCurrentCard().FlipCard();
            StartCoroutine(WaitAndFlip(player.GetCurrentCard()));
        }
        canDeal = true;
        ChangeCurrentPlayer();
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
            MoveCardToDiscard(card, currentPlayer);
            switch (card.tag)
            {
                case "Guard":
                    PlayGuard();
                    break;
                case "Priest":
                    PlayPriest();
                    break;
                case "Baron":
                    PlayBaron();
                    break;
                case "Handmaid":
                    PlayHandmaid();
                    break;
                case "Prince":
                    foreach (Card currentCard in currentPlayer.GetCurrentCards())
                    {
                        if (currentCard.GetValue() == 7)
                        {
                            break;
                        }
                    }
                    PlayPrince();
                    break;
                case "King":
                    foreach (Card currentCard in currentPlayer.GetCurrentCards())
                        {
                            if (currentCard.GetValue() == 7)
                            {
                                break;
                            }
                        }
                    PlayKing();
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
        }
    }

    public void PlayGuard()
    {
        DisplayPlayerButtonsGuard();
    }

    public void PlayPriest()
    {
        DisplayPlayerButtonsPriest();
    }

    public void PlayBaron()
    {
        DisplayPlayerButtonsBaron();
    }

    public void PlayHandmaid()
    {
        currentPlayer.SetInvincible(true);
        ChangeCurrentPlayer();
    }

    public void PlayPrince()
    {
        DisplayPlayerButtonsPrince();
    }

    public void PlayKing()
    {
        DisplayPlayerButtonsKing();
    }

    public void PlayCountess()
    {
        ChangeCurrentPlayer();
    }

    public void PlayPrincess()
    {
        currentPlayer.SetActive(false);
        ChangeCurrentPlayer();
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

            Player currentPlayer = players[currentPlayerNumber];
            int previousPlayerNumber = (currentPlayerNumber == 0) ? 3 : currentPlayerNumber - 1;

            Player previousPlayer = players[previousPlayerNumber];

            if (currentPlayer.GetCurrentCardsNumber() < 2 && previousPlayer.GetCurrentCardsNumber() < 2)
            {
                deck.DealCard(players[currentPlayerNumber]);
            } else {

                return;
            }
        }
    }
}
