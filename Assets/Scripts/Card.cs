using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int value;
    public int count;

    GameSession gameSession;

    [SerializeField] Sprite front;

    [SerializeField] Sprite back;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    public int GetValue()
    {
        return value;
    }

    public void FlipCard()
    {
        if (GetComponent<SpriteRenderer>().sprite == front)
        {
            GetComponent<SpriteRenderer>().sprite = back;
        } else 
        {
            GetComponent<SpriteRenderer>().sprite = front;
        }
    }

    public void DestroyCard()
    {
        Destroy(gameObject);
    }

    public Sprite GetBackImage()
    {
        return this.back;
    }

    public Sprite GetFrontImage()
    {
        return this.front;
    }

    public void ShowFrontImage()
    {
        GetComponent<SpriteRenderer>().sprite = front;
    }

    void OnMouseDown()
    {
        if (gameObject.tag == "Deck")
        {
            gameSession.DealCard();
        }
        else if (gameObject.tag != "Deck")
        {
            gameSession.PlayCard(this);
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (transform.rotation.y > 0)
    //     {
    //         GetComponent<SpriteRenderer>().sprite = back;
    //     } else {

    //     }
    // }
}
