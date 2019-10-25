using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] Card[] availableCards;

    [SerializeField] List<Card> deck;

    public Card[] visibleDeck;

    int cardsDealt = 0;

    public void SetUpDeck()
    {
        PopulateDeck();
        Shuffle(deck);
        DisplayCardDeck(deck);

    }

    void PopulateDeck()
    {
        foreach (Card card in availableCards)
        {
            int counter = card.count;
            while (counter > 0)
            {
                deck.Add(card);
                counter--;
            };
        };
    }

    public int NumberOfCards()
    {
        return deck.Count;
    }

    void Shuffle(List<Card> cards)
    {
        for (int i = cards.Count-1; i > 0; i--)
        {
            int rnd = UnityEngine.Random.Range(0,i);
            Card temp = cards[i];
            cards[i] = cards[rnd];
            cards[rnd] = temp;
        }
    }

    public void ClearDeck()
    {
        deck.Clear();
        cardsDealt = 0;
    }

    void DisplayCardDeck(List<Card> deck)
    {
        float counter = 0;
        float position = -1.5f;
        foreach (Card card in deck)
        {
            card.GetComponent<SpriteRenderer>().sprite = card.GetBackImage();
            card.gameObject.tag = "Deck";
            Instantiate(card, new Vector3(position, 0, -counter), Quaternion.identity);
            counter += 0.1f;
            position += 0.03f;
        }
        Array.Clear(visibleDeck, 0, visibleDeck.Length);
        visibleDeck = FindObjectsOfType<Card>();
    }

    public Card DealHiddenCard()
    {
        Vector3 newPos = new Vector3(1.5f, 0, -1);
        Card card = visibleDeck[cardsDealt];
        card.MoveCard(newPos, 0, 0.5f);
        deck.RemoveAt(deck.Count-1);
        cardsDealt++;
        return card;
    }

    public void DealCard(Player player)
    {
        Card card = visibleDeck[cardsDealt];
        player.AddCard(card);
        deck.RemoveAt(deck.Count-1);
        cardsDealt++;

    }

}
