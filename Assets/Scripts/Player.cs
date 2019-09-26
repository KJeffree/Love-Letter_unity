using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> currentCards;

    public bool active;

    public float xPos1;
    public float yPos1;

    public float xPos2;
    public float yPos2;

    public GameObject discardCardPile;
    public int cardRotation;

    int points;

    public int playerNumber;


    List<Card> playedCards;

    public bool invincible = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetInvincible(bool status)
    {
        invincible = status;
    }

    public GameObject GetDiscardPile()
    {
        return discardCardPile;
    }

    public int GetCurrentCardsNumber()
    {
        return currentCards.Count;
    }

    public void AddCard(Card card)
    {
        currentCards.Add(card);
        card.GetComponent<SpriteRenderer>().sprite = card.GetFrontImage();
        card.gameObject.tag = card.gameObject.transform.name;
        if (currentCards.Count == 1)
        {
            Instantiate(card, new Vector3(xPos1, yPos1, transform.position.z), Quaternion.Euler(0, 0, cardRotation));
        } else
        {
            Instantiate(card, new Vector3(xPos2, yPos2, transform.position.z - 1), Quaternion.Euler(0, 0, cardRotation));

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
