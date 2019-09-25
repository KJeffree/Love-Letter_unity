using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> currentCards;

    public bool active;

    int points;

    public int playerNumber;


    List<Card> playedCards;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddCard(Card card)
    {
        currentCards.Add(card);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
