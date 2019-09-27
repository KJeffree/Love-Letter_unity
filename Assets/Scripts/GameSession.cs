using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    [SerializeField] Player[] players;

    Deck deck;

    int currentPlayerNumber = 0;

    Player currentPlayer;

    [SerializeField] GameObject[] baronButtons;
    [SerializeField] GameObject[] guardButtons;
    [SerializeField] GameObject[] princeButtons;


    // Start is called before the first frame update
    void Start()
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

    private void DisplayPlayerButtonsBaron()
    {
        foreach (GameObject button in baronButtons)
        {
            button.SetActive(true);
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
        } else {
            return;
        }
    }

    public void PlayCard(Card card)
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

    public void PlayGuard(Card card)
    {
        Debug.Log("PlayingGuard");
        MoveCardToDiscard(card, currentPlayer);

    }
    public void PlayPriest(Card card)
    {
        Debug.Log("PlayingPriest");
        MoveCardToDiscard(card, currentPlayer);

    }
    public void PlayBaron(Card card)
    {
        // currentPlayer.WaitForPlayerChoice();
        DisplayPlayerButtonsBaron();
        MoveCardToDiscard(card, currentPlayer);
    }
    public void PlayHandmaid(Card card)
    {
        currentPlayer.SetInvincible(true);
        MoveCardToDiscard(card, currentPlayer);
        Debug.Log("PlayingHandmaid");
    }
    public void PlayPrince(Card card)
    {
        Debug.Log("PlayingPrince");
        MoveCardToDiscard(card, currentPlayer);
    }
    public void PlayKing(Card card)
    {
        Debug.Log("PlayingKing");
        MoveCardToDiscard(card, currentPlayer);

    }
    public void PlayCountess(Card card)
    {
        Debug.Log("PlayingCountess");
        MoveCardToDiscard(card, currentPlayer);        
    }
    public void PlayPrincess(Card card)
    {
        Debug.Log("PlayingPrincess");   
        MoveCardToDiscard(card, currentPlayer);
     
    }

    public void MoveCardToDiscard(Card card, Player player)
    {
        player.RemoveCard(card);
        Vector3 discardPosition = player.GetDiscardPile().transform.position;
        card.transform.position = discardPosition;
        ChangeCurrentPlayer();
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
