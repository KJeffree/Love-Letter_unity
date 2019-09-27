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
        // Debug.Log(player.GetCurrentCard());
        Debug.Log(player.GetCurrentCardValue());
        Debug.Log(currentPlayer.GetCurrentCardValue());

        if (player.GetCurrentCardValue() > currentPlayer.GetCurrentCardValue())
        {
            MoveCardToDiscard(currentPlayer.currentCards[0], currentPlayer);
            Debug.Log("target higher value");
            currentPlayer.SetActive(false);
        } else if (player.GetCurrentCardValue() < currentPlayer.GetCurrentCardValue())
        {
            Debug.Log("player higher value");
            MoveCardToDiscard(player.currentCards[0], player);
            player.SetActive(false);
        } else {
            Debug.Log("Both equal");
            return;
        }
        ChangeCurrentPlayer();
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
        ChangeCurrentPlayer();
    }
    public void PlayPriest(Card card)
    {
        Debug.Log("PlayingPriest");
        MoveCardToDiscard(card, currentPlayer);
        ChangeCurrentPlayer();
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
        ChangeCurrentPlayer();
    }
    public void PlayPrince(Card card)
    {
        Debug.Log("PlayingPrince");
        MoveCardToDiscard(card, currentPlayer);
        ChangeCurrentPlayer();
    }
    public void PlayKing(Card card)
    {
        Debug.Log("PlayingKing");
        MoveCardToDiscard(card, currentPlayer);
        ChangeCurrentPlayer();
    }
    public void PlayCountess(Card card)
    {
        Debug.Log("PlayingCountess");
        MoveCardToDiscard(card, currentPlayer);
        ChangeCurrentPlayer();
    }
    public void PlayPrincess(Card card)
    {
        Debug.Log("PlayingPrincess");   
        MoveCardToDiscard(card, currentPlayer);
        ChangeCurrentPlayer();
    }

    public void MoveCardToDiscard(Card card, Player player)
    {
        player.RemoveCard(card);
        Debug.Log(player);
        player.AddToPlayedCards(card);
        Vector3 discardPile = player.GetDiscardPile().transform.position;
        Vector3 discardPosition = new Vector3(0, 0, 0);
        if (player.GetPlayedCardsNumber() > 1)
        {
            if (player.GetNumber() == 1)
            {
                discardPosition = new Vector3(discardPile.x + (player.GetPlayedCardsNumber() - 1), discardPile.y, discardPile.z - (player.GetPlayedCardsNumber() - 1));
            } else if (player.GetNumber() == 2)
            {
                discardPosition = new Vector3(discardPile.x, discardPile.y - (player.GetPlayedCardsNumber() - 1), discardPile.z - (player.GetPlayedCardsNumber() - 1));            
            } else if (player.GetNumber() == 3)
            {
                discardPosition = new Vector3(discardPile.x - (player.GetPlayedCardsNumber() - 1), discardPile.y, discardPile.z - (player.GetPlayedCardsNumber() - 1));            
            } else if (player.GetNumber() == 4)
            {
                discardPosition = new Vector3(discardPile.x, discardPile.y + (player.GetPlayedCardsNumber() - 1), discardPile.z - (player.GetPlayedCardsNumber() - 1));            
            }
        } else {
            discardPosition = player.GetDiscardPile().transform.position;
        }
        card.transform.position = discardPosition;

        player.PositionSingleCard();

        // ChangeCurrentPlayer();
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
