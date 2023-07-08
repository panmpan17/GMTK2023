using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPack;
using TMPro;

public class CardChooser : MonoBehaviour
{
    public static CardChooser ins;

    [SerializeField]
    private Card cardPrefab;

    [SerializeField]
    private Vector2 firstCardPosition;
    [SerializeField]
    private Vector2 finalCardPosition;
    [SerializeField]
    private IntReference maxChooseCount;


    private Card[] _currentCards;
    private List<Card> _selectedCards;

    void Awake()
    {
        ins = this;

        _selectedCards = new List<Card>(maxChooseCount.Value);
    }


    public void SpawnCard(CardType[] cardTypes)
    {
        _currentCards = new Card[cardTypes.Length];

        int length = cardTypes.Length;
        for (int i = 0; i < length; i++)
        {
            Card card = Instantiate(cardPrefab, transform);
            card.Setup(cardTypes[i],  Vector2.Lerp(firstCardPosition, finalCardPosition, (float)i / (length - 1)));

            card.OnClick += OnCardClick;
            card.OnDiscard += OnCardDiscard;

            _currentCards[i] = card;
        }
    }

    public void SpawnCardRest(CardType[] cardTypes)
    {
        int cardTypeIndex = 0;
        int length = _currentCards.Length;
        for (int i = 0; i < length; i++)
        {
            if (_currentCards[i])
                continue;

            Card card = Instantiate(cardPrefab, transform);
            card.Setup(cardTypes[cardTypeIndex++], Vector2.Lerp(firstCardPosition, finalCardPosition, (float)i / (length - 1)));

            card.OnClick += OnCardClick;
            card.OnDiscard += OnCardDiscard;

            _currentCards[i] = card;
        }

        enabled = true;
    }

    void RemoveCardFromSelected(Card card)
    {
        _selectedCards.Remove(card);
        card.Unselect();

        for (int i = 0; i < _selectedCards.Count; i++)
        {
            _selectedCards[i].Select(i + 1);
        }
    }

    public void OnCardClick(Card card)
    {
        if (!enabled)
            return;

        if (_selectedCards.Contains(card))
        {
            RemoveCardFromSelected(card);
        }
        else
        {
            if (_selectedCards.Count >= maxChooseCount.Value)
                return;

            if (card.IsDiscard)
                card.UnDiscard();

            _selectedCards.Add(card);
            card.Select(_selectedCards.Count);
        }
    }

    public void OnCardDiscard(Card card)
    {
        if (!enabled)
            return;
        if (!card.IsDiscard)
            return;

        if (card.IsSelected)
            RemoveCardFromSelected(card);
    }

    public void Submit()
    {
        if (_selectedCards.Count < maxChooseCount.Value)
        {
            return;
        }

        CardType[] seletedCardTypes = new CardType[_selectedCards.Count];
        for (int i = 0; i < _selectedCards.Count; i++)
        {
            seletedCardTypes[i] = _selectedCards[i].CardType;
        }

        for (int i = 0; i < _currentCards.Length; i++)
        {
            if (_currentCards[i].IsSelected)
            {
                Destroy(_currentCards[i].gameObject);
                _currentCards[i] = null;
            }
        }

        enabled = false;
        _selectedCards.Clear();
        GameManager.ins.ConfirmChoosedCards(seletedCardTypes);
    }
    // private Card
}
