using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    GameSession gameSession;
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    public void KingTargetChosen(Player player)
    {
        Player currentPlayer = gameSession.GetCurrentPlayer();
        if (player != null)
        {
            gameSession.UpdateGamePlayText("King played on Player " + player.GetNumber());
            Card currentPlayerCard = currentPlayer.GetHand().GetCurrentCard();
            Card targetPlayerCard = player.GetHand().GetCurrentCard();
            currentPlayer.SwapCard(targetPlayerCard, player);
            player.SwapCard(currentPlayerCard, currentPlayer);
        }
        gameSession.DisablePlayerButtons();
        gameSession.SetCanDeal(true);      
        gameSession.ChangeCurrentPlayer();
    }

}
