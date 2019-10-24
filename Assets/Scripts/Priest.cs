using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : MonoBehaviour
{
    GameSession gameSession;
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    public void PriestTargetChosen(Player player)
    {
        if (player != null)
        {
            gameSession.UpdateGamePlayText("Priest played on Player " + player.GetNumber());
        }
        Player currentPlayer = gameSession.GetCurrentPlayer();
        if (player != null && currentPlayer.GetNumber() == 1)
        {
            player.GetHand().GetCurrentCard().FlipCard();
            gameSession.DisablePlayerButtons();
            StartCoroutine(WaitAndFlip(player.GetHand().GetCurrentCard()));
        } else 
        {
            gameSession.SetCanDeal(true);
            gameSession.ChangeCurrentPlayer();
        }
    }

    IEnumerator WaitAndFlip(Card card)
    {
        yield return new WaitForSeconds(3);
        card.FlipCard();
        gameSession.SetCanDeal(true);
        gameSession.ChangeCurrentPlayer();
    }
}
