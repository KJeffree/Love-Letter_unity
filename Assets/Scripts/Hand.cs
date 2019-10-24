using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    public List<Card> currentCards = new List<Card>();

    public List<Card> playedCards = new List<Card>();

    public float xPos1;
    public float yPos1;

    public float xPos2;
    public float yPos2;

    public GameObject discardCardPile;

    public int cardRotation;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearCards()
    {
        currentCards.Clear();
        playedCards.Clear();
    }

    public int GetCardRotation()
    {
        return cardRotation;
    }

    public int GetTotalValueOfPlayedCards()
    {
        int total = 0;
        foreach (Card card in playedCards)
        {
            total += card.GetValue();
        }
        return total;
    }

    public GameObject GetDiscardPile()
    {
        return discardCardPile;
    }

    public Card GetCurrentCard()
    {
        return currentCards[0];
    }

     public List<Card> GetCurrentCards()
    {
        return currentCards;
    }

    public int GetCurrentCardsNumber()
    {
        return currentCards.Count;
    }

    public int GetCurrentCardValue()
    {
        return currentCards[0].GetValue();
    }

    public void RemoveCard(Card card)
    {
        currentCards.Remove(card);
    }
    public void AddToPlayedCards(Card card)
    {
        playedCards.Add(card);
    }

    public int GetPlayedCardsNumber()
    {
        return playedCards.Count;
    }

    public void AddCard(Card card)
    {
        if (currentCards.Count == 0)
        {
            currentCards.Add(card);
            Vector3 newPos = new Vector3(xPos1, yPos1, transform.position.z);
            card.MoveCard(newPos, cardRotation, 0.5f);
        } else
        {
            currentCards.Add(card);
            Vector3 newPos = new Vector3(xPos2, yPos2, transform.position.z - 1);
            card.MoveCard(newPos, cardRotation, 0.5f);
        }
    }

    public void SwapCard(Card card, Player currentPlayer)
    {
        currentCards.RemoveAt(0);
        currentCards.Add(card);
        card.MoveCard(currentPlayer.transform.position, cardRotation, 1);
    }

    public void PositionSingleCard()
    {
        if (currentCards.Count > 0)
        {
            currentCards[0].transform.position = new Vector3(xPos1, yPos1, transform.position.z);
        }
    }

    public void MoveCardToDiscard(Card card)
    {
        Player player = GetComponentInParent<Player>();
        card.ShowFrontImage();
        RemoveCard(card);
        AddToPlayedCards(card);
        Vector3 discardPile = discardCardPile.transform.position;
        float cardDisplacement = (float)(GetPlayedCardsNumber() * 0.5 - 0.5);
        int zPositionAlteration = GetPlayedCardsNumber() - 1;
        discardPile.z -= zPositionAlteration;
        if (GetPlayedCardsNumber() > 1)
        {
            if (player.GetNumber() == 1)
            {
                discardPile.x += cardDisplacement;
            } else if (player.GetNumber() == 2)
            {
                discardPile.y -= cardDisplacement;
            } else if (player.GetNumber() == 3)
            {
                discardPile.x -= cardDisplacement;
            } else if (player.GetNumber() == 4)
            {
                discardPile.y += cardDisplacement;
            }
        }
        card.MoveCard(discardPile, cardRotation, 0.1f);
        PositionSingleCard();
        if (card.GetValue() == 8)
        {
            player.SetActive(false);
            if (player.GetHand().GetCurrentCards().Count > 0)
            {
                MoveCardToDiscard(player.GetHand().GetCurrentCard());  
            }
        }
    }
    
}
