using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameState : ScriptableObject
{
    public Rules rules;
    public int randomSeed;

    public List<Card> player1Hand;
    public List<Card> player2Hand;

    public Player? localPlayer;
    public Phase player1Phase;
    public Phase player2Phase;

    public List<Card> deck { get; private set; }

    public void Init()
    {
        deck = Util.DeckFromConfig(rules);
        // TODO: deal player hand
    }
}
