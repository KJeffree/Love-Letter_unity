using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : MonoBehaviour
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
    public void PriestTargetChosen(Player player)
    {
        if (player != null)
        {
            gameSession.UpdateGamePlayText("Priest played on Player " + player.GetNumber());
        }
        Player currentPlayer = gameSession.GetCurrentPlayer();
        if (player != null && currentPlayer.GetNumber() == 1)
        {
            player.GetCurrentCard().FlipCard();
            gameSession.DisablePlayerButtons();
            StartCoroutine(WaitAndFlip(player.GetCurrentCard()));
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
