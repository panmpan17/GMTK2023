using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPack;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;

    [SerializeField]
    private CardDecks cardDecks;
    [SerializeField]
    private CardChooser cardChooser;
    [SerializeField]
    private PlayerDisplay playerDisplay;

    [SerializeField]
    private PlayerScenario[] playerScenarios;
    [SerializeField]
    private IntReference cardCount;

    private List<CardType> unusedCards;
    private List<CardType> discardedCards;

    private PlayerScenario.Player _currentPlayer;
    public PlayerScenario.Player CurrentPlayer => _currentPlayer;

    void Awake()
    {
        ins = this;

        var allCards = cardDecks.TotalCards;
        unusedCards = new List<CardType>(allCards.Length);
        unusedCards.AddRange(allCards);

        discardedCards = new List<CardType>(allCards.Length);
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        CardType[] cards = new CardType[cardCount.Value];
        for (int i = 0; i < cardCount.Value; i++)
        {
            cards[i] = GetRandomCard();
        }

        cardChooser.SpawnCard(cards);
        HUDDisplay.ins.UpdateCardsCount(unusedCards.Count, discardedCards.Count);

        _currentPlayer = playerScenarios[Random.Range(0, playerScenarios.Length)].GetRandomPlayer();
        playerDisplay.Display(_currentPlayer);
    }

    CardType GetRandomCard()
    {
        int index = Random.Range(0, unusedCards.Count);
        var card = unusedCards[index];
        unusedCards.RemoveAt(index);

        if (unusedCards.Count <= 0)
        {
            unusedCards.AddRange(discardedCards);
            discardedCards.Clear();
        }

        return card;
    }



    public void DiscardCards(CardType[] discardCardTypes)
    {
        discardedCards.AddRange(discardCardTypes);
        HUDDisplay.ins.UpdateCardsCount(unusedCards.Count, discardedCards.Count);
    }

    public void NewPlayerAndRedrawCard(int needCardCount)
    {
        CardType[] cards = new CardType[needCardCount];
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = GetRandomCard();
        }

        cardChooser.SpawnCardRest(cards);
        HUDDisplay.ins.UpdateCardsCount(unusedCards.Count, discardedCards.Count);

        _currentPlayer = playerScenarios[Random.Range(0, playerScenarios.Length)].GetRandomPlayer();
        playerDisplay.Display(_currentPlayer);
    }
}
