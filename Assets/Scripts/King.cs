using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void KingTargetChosen(Player player)
    {
        Player currentPlayer = gameSession.GetCurrentPlayer();
        if (player != null)
        {
            gameSession.UpdateGamePlayText("King played on Player " + player.GetNumber());
            Card currentPlayerCard = currentPlayer.GetCurrentCard();
            Card targetPlayerCard = player.GetCurrentCard();
            currentPlayer.SwapCard(targetPlayerCard);
            player.SwapCard(currentPlayerCard);
        }
        gameSession.DisablePlayerButtons();
        gameSession.SetCanDeal(true);      
        gameSession.ChangeCurrentPlayer();
    }

}
