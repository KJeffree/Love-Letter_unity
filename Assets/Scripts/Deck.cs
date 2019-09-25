using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] Card[] availableCards;

    [SerializeField] List<Card> deck;

    // Start is called before the first frame update
    void Start()
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
        
        Shuffle(deck);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
