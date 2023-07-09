using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MPack;
using TMPro;

public class CardChooser : MonoBehaviour
{
    public static CardChooser ins;

    [SerializeField]
    private Card cardPrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Button submitButton;

    [SerializeField]
    private float firstCardRotation;
    [SerializeField]
    private float finalCardRotation;
    [SerializeField]
    private RangeReference cardPositionX;
    [SerializeField]
    private RangeReference cardPositionY;
    [SerializeField]
    private AnimationCurve cardPositionYCurve;

    [SerializeField]
    private IntReference maxChooseCount;

    [SerializeField]
    private TextMeshProUGUI cardCountText;
    [SerializeField]
    private string cardCountTextFormat;


    private Card[] _currentCards;
    private List<Card> _selectedCards;

    void Awake()
    {
        ins = this;

        _selectedCards = new List<Card>(maxChooseCount.Value);
        submitButton.interactable = false;
        cardCountText.text = string.Format(cardCountTextFormat, 0, maxChooseCount.Value);
    }


    public void SpawnCard(CardType[] cardTypes)
    {
        _currentCards = new Card[cardTypes.Length];
        StartCoroutine(SpawnCardIEnumerator(cardTypes));
    }

    public void SpawnCardRest(CardType[] cardTypes)
    {
        StartCoroutine(SpawnCardIEnumerator(cardTypes));
    }
    IEnumerator SpawnCardIEnumerator(CardType[] cardTypes)
    {
        int cardTypeIndex = 0;
        int length = _currentCards.Length;
        for (int i = 0; i < length; i++)
        {
            if (_currentCards[i])
            {
                _currentCards[i].transform.SetSiblingIndex(i);
                continue;
            }

            float percentage = (float)i / (length - 1);

            Card card = Instantiate(cardPrefab, transform);
            card.transform.SetSiblingIndex(i);
            card.transform.position = spawnPoint.position;
            // card.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(firstCardRotation, finalCardRotation, percentage));

            float rotationAngle = Mathf.Lerp(firstCardRotation, finalCardRotation, percentage);
            var position = new Vector2(cardPositionX.Lerp(percentage), cardPositionY.Lerp(cardPositionYCurve.Evaluate(percentage)));
            card.Setup(cardTypes[cardTypeIndex++], position, rotationAngle);

            card.OnClick += OnCardClick;
            card.OnDiscard += OnCardDiscard;

            _currentCards[i] = card;

            yield return new WaitForSeconds(0.4f);
        }

        enabled = true;
    }

    void RemoveCardFromSelected(Card card)
    {
        _selectedCards.Remove(card);
        card.Unselect();

        for (int i = 0; i < _selectedCards.Count; i++)
        {
            _selectedCards[i].Select(i);
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

            card.Select(_selectedCards.Count);
            _selectedCards.Add(card);
        }

        cardCountText.text = string.Format(cardCountTextFormat, _selectedCards.Count, maxChooseCount.Value);
        submitButton.interactable = _selectedCards.Count >= maxChooseCount.Value;
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

        List<CardType> discards = new List<CardType>();
        for (int i = 0; i < _currentCards.Length; i++)
        {
            if (_currentCards[i].IsSelected)
                _currentCards[i] = null;
            else if (_currentCards[i].IsDiscard)
            {
                _currentCards[i].DiscardAndDestroy();
                discards.Add(_currentCards[i].CardType);
                _currentCards[i] = null;
            }
        }

        enabled = false;

        if (discards.Count > 0)
        {
            GameManager.ins.DiscardCards(discards.ToArray());
        }
        TellStoryDisplay.ins.StartTelling(_selectedCards.ToArray(), discards.Count > 0);

        _selectedCards.Clear();
        submitButton.interactable = false;
        cardCountText.text = "";
    }

    public void Redraw()
    {
        int cardNeed = 0;
        for (int i = 0; i < _currentCards.Length; i++)
        {
            if (_currentCards[i])
                continue;

            cardNeed++;
        }

        GameManager.ins.NewPlayerAndRedrawCard(cardNeed);
        cardCountText.text = string.Format(cardCountTextFormat, 0, maxChooseCount.Value);
    }

    public void HideAllCards()
    {
        for (int i = 0; i < _currentCards.Length; i++)
        {
            if (_currentCards[i])
                _currentCards[i].gameObject.SetActive(false);
        }
        submitButton.gameObject.SetActive(false);
    }
}
