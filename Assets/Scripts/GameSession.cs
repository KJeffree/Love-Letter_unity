using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    [SerializeField] Player[] players;

    Deck deck;

    int target = 0;
    // Start is called before the first frame update
    void Start()
    {
    
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DealCard();
        }
    }

    void DealCard()
    {
        deck = FindObjectOfType<Deck>();
        deck.DealCard(players[target]);
        if (target < 3)
        {
            target++;
        } else
        {
            target = 0;
        }
    }
}
