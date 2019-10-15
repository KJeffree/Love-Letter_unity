using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prince : MonoBehaviour
{
    GameSession gameSession;
    Deck deck;
    Card hiddenCard;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        deck = FindObjectOfType<Deck>();
        hiddenCard = gameSession.GetHiddenCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrinceTargetChosen(Player player)
    {
        gameSession.UpdateGamePlayText("Prince played on Player " + player.GetNumber());
        if (player.GetHand().GetCurrentCard().GetValue() == 8)
        {
            gameSession.MoveCardToDiscard(player.GetHand().GetCurrentCard(), player);
        } else {
            gameSession.MoveCardToDiscard(player.GetHand().GetCurrentCard(), player);
            if (deck.NumberOfCards() > 0)
            {
                deck.DealCard(player);
            } else
            {
                player.AddCard(hiddenCard);
                Destroy(hiddenCard.gameObject);
            }
        }
        gameSession.DisablePlayerButtons();
        gameSession.SetCanDeal(true);
        gameSession.ChangeCurrentPlayer();
    }
}
