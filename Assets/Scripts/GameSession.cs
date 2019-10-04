﻿using System.Collections;
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

    Card hiddenCard;
    Card hiddenCardVisible;



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
        MoveCardToDiscard(player.GetCurrentCard(), player);
        if (deck.NumberOfCards() > 0)
        {
            deck.DealCard(player);
        } else
        {
            player.AddCard(hiddenCard);
            Destroy(hiddenCardVisible.gameObject);
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
        MoveCardToDiscard(card, currentPlayer);
        ChangeCurrentPlayer();
    }
    public void PlayPriest(Card card)
    {
        MoveCardToDiscard(card, currentPlayer);
        ChangeCurrentPlayer();
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
        DisplayPlayerButtonsPrince();
        MoveCardToDiscard(card, currentPlayer);
    }
    public void PlayKing(Card card)
    {
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
        ChangeCurrentPlayer();
    }

    public void MoveCardToDiscard(Card card, Player player)
    {
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
