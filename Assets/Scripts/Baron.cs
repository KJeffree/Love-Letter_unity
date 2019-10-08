using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baron : MonoBehaviour
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

    public void BaronTargetChosen(Player player)
    {
        Player currentPlayer = gameSession.GetCurrentPlayer();
        if (player != null)
        {
            if (player.GetCurrentCardValue() > currentPlayer.GetCurrentCardValue())
            {
                gameSession.MoveCardToDiscard(currentPlayer.currentCards[0], currentPlayer);
                currentPlayer.SetActive(false);
            } else if (player.GetCurrentCardValue() < currentPlayer.GetCurrentCardValue())
            {
                gameSession.MoveCardToDiscard(player.currentCards[0], player);
                player.SetActive(false);
            }
        }
        gameSession.DisablePlayerButtons();
        gameSession.SetCanDeal(true);
        gameSession.ChangeCurrentPlayer();
    }
}
