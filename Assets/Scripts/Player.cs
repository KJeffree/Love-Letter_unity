using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public List<Card> currentCards;

    public bool active = true;

    public float xPos1;
    public float yPos1;

    public float xPos2;
    public float yPos2;

    public GameObject discardCardPile;

    public int cardRotation;

    public int points;

    public int playerNumber;

    public TextMeshProUGUI pointsText;

    public List<Card> playedCards = new List<Card>();

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
        currentCards.Clear();
        playedCards.Clear();
        active = true;
        invincible = false;
    }

    public int TotalValueOfPlayedCards()
    {
        int total = 0;
        foreach (Card card in playedCards)
        {
            total += card.GetValue();
        }
        return total;
    }

    public GameObject GetDiscardPile()
    {
        return discardCardPile;
    }

    public bool GetActive()
    {
        return active;
    }

    public bool GetInvincible()
    {
        return invincible;
    }

    public Card GetCurrentCard()
    {
        return currentCards[0];
    }

    public List<Card> GetCurrentCards()
    {
        return currentCards;
    }

    public int GetCurrentCardsNumber()
    {
        return currentCards.Count;
    }

    public int GetCurrentCardValue()
    {
        return currentCards[0].GetValue();
    }

    public void RemoveCard(Card card)
    {
        currentCards.Remove(card);
    }

    public void AddToPlayedCards(Card card)
    {
        playedCards.Add(card);
    }

    public int GetPlayedCardsNumber()
    {
        return playedCards.Count;
    }

    public void AddCard(Card card)
    {
        card.GetComponent<SpriteRenderer>().sprite = playerNumber == 1 ? card.GetFrontImage() : card.GetBackImage() ;
        card.gameObject.tag = card.gameObject.transform.name;
        if (currentCards.Count == 0)
        {
            currentCards.Add(Instantiate(card, new Vector3(xPos1, yPos1, transform.position.z), Quaternion.Euler(0, 0, cardRotation)));
        } else
        {
            currentCards.Add(Instantiate(card, new Vector3(xPos2, yPos2, transform.position.z - 1), Quaternion.Euler(0, 0, cardRotation)));
        }
    }

    public void SwapCard(Card card, Player otherPlayer)
    {
        currentCards.RemoveAt(0);
        currentCards.Add(card);
        // card.transform.position = new Vector3(xPos1, yPos1, transform.position.z);
        card.transform.rotation = Quaternion.Euler(0, 0, cardRotation);
        StartCoroutine(MoveToPosition(gameObject.GetComponent<Transform>(), otherPlayer.GetComponent<Transform>(), 1, card));
        if (playerNumber == 1)
        {
            card.GetComponent<SpriteRenderer>().sprite = card.GetFrontImage();
        } else 
        {
            card.GetComponent<SpriteRenderer>().sprite = card.GetBackImage();

        }
    }

    IEnumerator MoveToPosition(Transform player, Transform otherPlayer, float timeToMove, Card card)
    {
        var currentPos = otherPlayer.position;
        var newPos = player.position;
        var t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            card.transform.position = Vector3.Lerp(currentPos, newPos, t);
            yield return null;
        }
    }
    
    public void PositionSingleCard()
    {
        if (currentCards.Count > 0)
        {
            currentCards[0].transform.position = new Vector3(xPos1, yPos1, transform.position.z);
        }
    }
    
}
