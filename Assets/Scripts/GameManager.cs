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
    private PlayerScenario[] playerScenarios;
    [SerializeField]
    private IntReference cardCount;

    [SerializeField]
    private IntReference maxSuccessCount;
    [SerializeField]
    private IntReference maxFailCount;

    private List<CardType> unusedCards;
    private List<CardType> discardedCards;

    private PlayerScenario.Player _currentPlayer;
    public PlayerScenario.Player CurrentPlayer => _currentPlayer;

    private int _successCount, _failCount;

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
        PlayerDisplay.ins.Display(_currentPlayer);

        HUDDisplay.ins.UpdateSuccessFailCount(0, 0);
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


    public void DiscardCards(CardType discardCardType)
    {
        discardedCards.Add(discardCardType);
        HUDDisplay.ins.UpdateCardsCount(unusedCards.Count, discardedCards.Count);
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
        PlayerDisplay.ins.Display(_currentPlayer);
    }

    public bool Success()
    {
        _successCount++;
        HUDDisplay.ins.UpdateSuccessFailCount(_successCount, _failCount);

        if (_successCount >= maxSuccessCount.Value)
        {
            End.ins.Show(true);
            return true;
        }
        return false;
    }

    public bool Fail()
    {
        _failCount++;
        HUDDisplay.ins.UpdateSuccessFailCount(_successCount, _failCount);

        if (_failCount >= maxFailCount.Value)
        {
            End.ins.Show(false);
            return true;
        }
        return false;
    }
}
