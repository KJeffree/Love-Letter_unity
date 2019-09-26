using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    [SerializeField] Player[] players;

    Deck deck;

    int currentPlayerNumber = 0;

    Player currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        deck = FindObjectOfType<Deck>();

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
        currentPlayer.SetInvincible(false);
    }

    public void PlayCard(Card card)
    {
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
                PlayPrince();
                break;
            case "King":
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

    public void PlayGuard()
    {
        Debug.Log("PlayingGuard");
    }
    public void PlayPriest()
    {
        Debug.Log("PlayingPriest");
        
    }
    public void PlayBaron()
    {
        Debug.Log("PlayingBaron");
        
    }
    public void PlayHandmaid()
    {
        currentPlayer.SetInvincible(true);
        Debug.Log("PlayingHandmaid");
    }
    public void PlayPrince()
    {
        Debug.Log("PlayingPrince");
        
    }
    public void PlayKing()
    {
        Debug.Log("PlayingKing");
        
    }
    public void PlayCountess()
    {
        Debug.Log("PlayingCountess");
        
    }
    public void PlayPrincess()
    {
        Debug.Log("PlayingPrincess");        
    }

    public void MoveCardToDiscard(Card card)
    {
        Player player = players[currentPlayerNumber];
        Vector3 discardPosition = player.GetDiscardPile().transform.position;
        card.transform.position = discardPosition;
    }

    public void DealCard()
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
