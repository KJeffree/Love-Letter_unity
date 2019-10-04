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
        // causing stack overflow
        
        if (players[currentPlayerNumber].GetActive() == false)
        {
            ChangeCurrentPlayer();
        }
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
            Debug.Log(players[i+1]);
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
        ChangeCurrentPlayer();
        canDeal = true;
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
        ChangeCurrentPlayer();
        canDeal = true;
    }

    public void KingTargetChosen(Player player)
    {
        Card currentPlayerCard = currentPlayer.GetCurrentCard();
        Card targetPlayerCard = player.GetCurrentCard();
        currentPlayer.SwapCard(targetPlayerCard);
        player.SwapCard(currentPlayerCard);
        DisablePlayerButtons();
        ChangeCurrentPlayer(); 
        canDeal = true;      
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
        ChangeCurrentPlayer();
        canDeal = true;
    }

    public void PriestTargetChosen(Player player)
    {
        player.GetCurrentCard().FlipCard();
        StartCoroutine(WaitAndFlip(player.GetCurrentCard()));
        DisablePlayerButtons();
        ChangeCurrentPlayer();
        canDeal = true;
    }

    IEnumerator WaitAndFlip(Card card)
    {
        yield return new WaitForSeconds(3);
        card.FlipCard();
    }

    public void PlayCard(Card card)
    {
        if (currentPlayer.GetCurrentCardsNumber() == 2 && currentPlayer.GetCurrentCards().Contains(card))
        {
            switch (card.tag)
            {
                case "Guard":
                    PlayGuard(card);
                    break;
                case "Priest":
                    PlayPriest(card);
                    break;
                case "Baron":
                    PlayBaron(card);
                    break;
                case "Handmaid":
                    PlayHandmaid(card);
                    break;
                case "Prince":
                    PlayPrince(card);
                    break;
                case "King":
                    PlayKing(card);
                    break;
                case "Countess":
                    PlayCountess(card);
                    break;
                case "Princess":
                    PlayPrincess(card);
                    break;
                default:
                    break;
            }
        }
    }

    public void PlayGuard(Card card)
    {
        DisplayPlayerButtonsGuard();
        MoveCardToDiscard(card, currentPlayer);
    }
    public void PlayPriest(Card card)
    {
        DisplayPlayerButtonsPriest();
        MoveCardToDiscard(card, currentPlayer);
    }
    public void PlayBaron(Card card)
    {
        DisplayPlayerButtonsBaron();
        MoveCardToDiscard(card, currentPlayer);
    }
    public void PlayHandmaid(Card card)
    {
        currentPlayer.SetInvincible(true);
        MoveCardToDiscard(card, currentPlayer);
        ChangeCurrentPlayer();
    }
    public void PlayPrince(Card card)
    {
        foreach (Card currentCard in currentPlayer.GetCurrentCards())
        {
            if (currentCard.GetValue() == 7 || currentCard.GetValue() == 7)
            {
                return;
            }
        }
        DisplayPlayerButtonsPrince();
        MoveCardToDiscard(card, currentPlayer);
    }
    public void PlayKing(Card card)
    {
        foreach (Card currentCard in currentPlayer.GetCurrentCards())
        {
            if (currentCard.GetValue() == 7 || currentCard.GetValue() == 7)
            {
                return;
            }
        }
        DisplayPlayerButtonsKing();
        MoveCardToDiscard(card, currentPlayer);
    }
    public void PlayCountess(Card card)
    {
        MoveCardToDiscard(card, currentPlayer);
        ChangeCurrentPlayer();
    }
    public void PlayPrincess(Card card)
    {
        MoveCardToDiscard(card, currentPlayer);
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
