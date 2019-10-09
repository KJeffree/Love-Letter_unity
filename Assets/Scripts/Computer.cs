using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
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

    public void TakeTurn(Player currentPlayer, List<Player> targets)
    {
        gameSession.DealCard();
        Card chosenCard = ChooseCard(currentPlayer);
        Player chosenPlayer = ChoosePlayer(targets);
        StartCoroutine(WaitAndPlayCard(chosenCard, chosenPlayer));
    }

    IEnumerator WaitAndPlayCard(Card chosenCard, Player chosenPlayer)
    {
        yield return new WaitForSeconds(2);
        if (chosenCard.GetValue() == 4 || chosenCard.GetValue() == 7 || chosenCard.GetValue() == 8)
        {
            gameSession.PlayCard(chosenCard);
        } else {
            gameSession.PlayCardComputer(chosenCard, chosenPlayer);
        }
    }

    private Player ChoosePlayer(List<Player> targets)
    {
        Player chosenPlayer = null;
        if (targets.Count == 0)
        {
            chosenPlayer = null;
        } else if (targets.Count == 1)
        {
            chosenPlayer = targets[0];
        } else {
            int randomIndex = Random.Range(0, targets.Count - 1);
            chosenPlayer = targets[randomIndex];
        }
        return chosenPlayer;
    }

    private Card ChooseCard(Player currentPlayer)
    {
        Card chosenCard = null;
        Card card1 = currentPlayer.GetCurrentCards()[0];
        Card card2 = currentPlayer.GetCurrentCards()[1];

        if (card1.GetValue() == 7 && card2.GetValue() == 5 || card1.GetValue() == 7 && card2.GetValue() == 6)
        {
            chosenCard = card1;
        }
        else if (card2.GetValue() == 7 && card1.GetValue() == 5 || card2.GetValue() == 7 && card1.GetValue() == 6)
        {
            chosenCard = card2;
        }
        else if (card1.GetValue() == 3 && card2.GetValue() < 5)
        {
            chosenCard = card2;
        }
        else if (card2.GetValue() == 3 && card1.GetValue() < 5)
        {
            chosenCard = card1;
        }
        else if (card1.GetValue() > card2.GetValue())
        {
            chosenCard = card2;
        } else 
        {
            chosenCard = card1;
        }
        return chosenCard;
    }
}
