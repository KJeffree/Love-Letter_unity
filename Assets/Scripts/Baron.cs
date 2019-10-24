using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baron : MonoBehaviour
{
    GameSession gameSession;
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    public void BaronTargetChosen(Player player)
    {
        if (player != null)
        {
            gameSession.UpdateGamePlayText("Baron played on Player " + player.GetNumber());
        }
        Player currentPlayer = gameSession.GetCurrentPlayer();
        if (player != null)
        {
            if (player.GetHand().GetCurrentCardValue() > currentPlayer.GetHand().GetCurrentCardValue())
            {
                currentPlayer.GetHand().MoveCardToDiscard(currentPlayer.GetHand().GetCurrentCards()[0]);
                currentPlayer.SetActive(false);
            } else if (player.GetHand().GetCurrentCardValue() < currentPlayer.GetHand().GetCurrentCardValue())
            {
                player.GetHand().MoveCardToDiscard(player.GetHand().GetCurrentCards()[0]);
                player.SetActive(false);
            }
        }
        gameSession.DisablePlayerButtons();
        gameSession.SetCanDeal(true);
        gameSession.ChangeCurrentPlayer();
    }
}
