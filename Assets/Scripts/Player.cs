using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Hand hand;

    public bool active = true;

    public int points;

    public int playerNumber;

    public TextMeshProUGUI pointsText;

    public bool invincible = false;
 

    void Update()
    {
        pointsText.text = "Player " + playerNumber + ": " + points.ToString();
    }

    public int GetNumber()
    {
        return playerNumber;
    }

    public void SetInvincible(bool status)
    {
        invincible = status;
    }

    public void SetActive(bool status)
    {
        active = status;
    }

    public int GetPoints()
    {
        return points;
    }

    public void AddPoint()
    {
        points++;
    }

    public void NewRound()
    {
        hand.ClearCards();
        active = true;
        invincible = false;
    }

    public Hand GetHand()
    {
        return hand;
    }
    public bool GetActive()
    {
        return active;
    }

    public bool GetInvincible()
    {
        return invincible;
    }

    public void AddCard(Card card)
    {
        if (playerNumber == 1)
        {
            card.ShowFrontImage();
        } else 
        {
            card.ShowBackImage();
        }
        string name = card.gameObject.transform.name;
        card.SetTag(name.Substring(0, name.Length-7));
        hand.AddCard(card);
    }

    public void SwapCard(Card card, Player otherPlayer)
    {
        hand.SwapCard(card, this);
        if (playerNumber == 1 || otherPlayer.GetNumber() == 1)
        {
            card.FlipCard();
        }
    }

}
