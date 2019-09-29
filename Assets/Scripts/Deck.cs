using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] Card[] availableCards;

    [SerializeField] List<Card> deck;

    Card[] visibleDeck;

    int cardsDealt = 0;

    // Start is called before the first frame update
    void Start()
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

    void Shuffle(List<Card> cards)
    {
        for (int i = cards.Count-1; i > 0; i--)
        {
            int rnd = Random.Range(0,i);
            Card temp = cards[i];
            cards[i] = cards[rnd];
            cards[rnd] = temp;
        }
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
        visibleDeck = FindObjectsOfType<Card>();
    }

    public Card DealHiddenCard()
    {
        Card lastCard = deck[deck.Count-1];
        deck.RemoveAt(deck.Count-1);
        return lastCard;
    }

    public void DealCard(Player player)
    {
        Card lastCard = deck[deck.Count-1];
        Card card = visibleDeck[cardsDealt];
        Destroy(card.gameObject);
        player.AddCard(lastCard);
        deck.RemoveAt(deck.Count-1);
        cardsDealt++;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
