using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
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

    public void GuardTargetCardChosen(int cardValue, Player target)
    {
        if (target != null && target.GetCurrentCard().GetValue() == cardValue)
        {
            gameSession.MoveCardToDiscard(target.GetCurrentCard(), target);
            target.SetActive(false);
        }
        gameSession.DisablePlayerButtons();
        gameSession.ChangeCurrentPlayer();
    }

    public void PlayGuardComputer(Player player)
    {
        int cardValueGuess = Random.Range(2, 8);
        GuardTargetCardChosen(cardValueGuess, player);
        gameSession.SetCanDeal(true);
    }
}
