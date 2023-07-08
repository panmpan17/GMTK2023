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
    private PlayerScenario[] playerScenarios;
    [SerializeField]
    private CardChooser cardChooser;
    [SerializeField]
    private IntReference cardCount;

    private List<CardType> unusedCards;
    // private List<CardType> displayedCards;
    private List<CardType> discardedCards;

    void Awake()
    {
        ins = this;

        var allCards = cardDecks.TotalCards;
        unusedCards = new List<CardType>(allCards.Length);
        unusedCards.AddRange(allCards);

        discardedCards = new List<CardType>(allCards.Length);

        // displayedCards = new List<CardType>(cardCount.Value);
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


    public void ConfirmChoosedCards(CardType[] cardTypes)
    {
        discardedCards.AddRange(cardTypes);

        CardType[] cards = new CardType[cardTypes.Length];
        for (int i = 0; i < cardTypes.Length; i++)
        {
            cards[i] = GetRandomCard();
        }

        cardChooser.SpawnCardRest(cards);
        HUDDisplay.ins.UpdateCardsCount(unusedCards.Count, discardedCards.Count);

        // cardChooser.SpawnCard(cards);
    }
}
