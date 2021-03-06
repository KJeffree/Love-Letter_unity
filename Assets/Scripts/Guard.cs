﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    GameSession gameSession;
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    public void GuardTargetCardChosen(int cardValue, Player target)
    {
        if (target != null)
        {
            string cardName = GetCardName(cardValue);
            gameSession.UpdateGamePlayText("Guard played on Player " + target.GetNumber() + ", card guessed: " + cardName);
        }
        if (target != null && target.GetHand().GetCurrentCard().GetValue() == cardValue)
        {
            target.GetHand().MoveCardToDiscard(target.GetHand().GetCurrentCard());
            target.SetActive(false);
        }
        gameSession.DisablePlayerButtons();
        gameSession.ChangeCurrentPlayer();
    }

    public void PlayGuardComputer(Player player)
    {
        int cardValueGuess = Random.Range(2, 9);
        GuardTargetCardChosen(cardValueGuess, player);
        gameSession.SetCanDeal(true);
    }

    private string GetCardName(int cardValue)
    {
        switch (cardValue)
        {
            case 2:
                return "Priest";
            case 3:
                return "Baron";
            case 4:
                return "Handmaid";
            case 5:
                return "Prince";
            case 6:
                return "King";
            case 7:
                return "Countess";
            case 8:
                return "Princess";
            default:
                return "";
        }
    }
}
