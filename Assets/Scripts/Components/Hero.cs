using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public void PlayCards(IEnumerable<Card> cards)
    {
        foreach (var card in cards)
        {
            PlayCard(card);
        }
    }

    public void PlayCard(Card card)
    {
        Debug.Log($"Playing card: {card}");

        switch (card)
        {
            case Card.DoNothing:
                break;

            // TODO: check that movement cards are allowed

            case Card.MoveEast:
                transform.position += Vector3.right;
                break;

            case Card.MoveNorth:
                transform.position += Vector3.forward;
                break;

            case Card.MoveRandom:
                transform.position += MiscUtility.GetRandomDirection();
                break;

            case Card.MoveSouth:
                transform.position += Vector3.back;
                break;

            case Card.MoveWest:
                transform.position += Vector3.back;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(card), card, null);
        }
    }
}
