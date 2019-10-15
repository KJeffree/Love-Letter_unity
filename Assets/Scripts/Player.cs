using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Hand hand;

    public bool active = true;

    // public float xPos1;
    // public float yPos1;

    // public float xPos2;
    // public float yPos2;

    // public GameObject discardCardPile;

    // public int cardRotation;

    public int points;

    public int playerNumber;

    public TextMeshProUGUI pointsText;

    // public List<Card> playedCards = new List<Card>();
    //     public List<Card> currentCards = new List<Card>();


    public bool invincible = false;
    // Start is called before the first frame update
    void Start()
    {
    }

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
        card.SetTag(card.gameObject.transform.name);
        hand.AddCard(card);
    }

    public void SwapCard(Card card, Player otherPlayer)
    {
        hand.SwapCard(card, otherPlayer);
        StartCoroutine(MoveToPosition(otherPlayer, 1, card));
    }

    IEnumerator MoveToPosition(Player otherPlayer, float timeToMove, Card card)
    {
        var currentPos = otherPlayer.transform.position;
        var origRot = card.transform.rotation.eulerAngles;
        var newPos = gameObject.transform.position;
        var newRotZ = hand.GetCardRotation();
        var newRotation = origRot;
        var t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            card.transform.position = Vector3.Lerp(currentPos, newPos, t);
            card.transform.rotation = Quaternion.Lerp(Quaternion.Euler(origRot), Quaternion.Euler(0, 0, newRotZ), t);
            yield return null;
        }
        if (playerNumber == 1 || otherPlayer.GetNumber() == 1)
            {
                card.FlipCard();
            }
    }
    
}
