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
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    public int GetValue()
    {
        return value;
    }

    public void SetTag(string tag)
    {
        gameObject.tag = tag;
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

    public void ShowBackImage()
    {
        GetComponent<SpriteRenderer>().sprite = back;
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

    public void PositionCard(float xPos, float yPos, float zPoz){
        transform.position = new Vector3(xPos, yPos, zPoz);
    }

    public void MoveCard(Vector3 newPos, int newRot, float timeToMove)
    {
        StartCoroutine(MoveToPosition(newPos, newRot, timeToMove));
    }

    IEnumerator MoveToPosition(Vector3 newPos, int newRot, float timeToMove)
    {
        Vector3 currentPos = gameObject.transform.position;
        Vector3 origRot = gameObject.transform.rotation.eulerAngles;
        var t = 0f;

        while(t < 1)
        {
            t += Time.deltaTime / timeToMove;
            gameObject.transform.position = Vector3.Lerp(currentPos, newPos, t);
            gameObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(origRot), Quaternion.Euler(0, 0, newRot), t);
            yield return null;
        }
    }

}
