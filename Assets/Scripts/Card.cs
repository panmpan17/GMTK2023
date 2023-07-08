using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image outterImage;
    [SerializeField]
    private Image innerImage;

    [SerializeField]
    private GameObject selectedNumber;
    [SerializeField]
    private TextMeshProUGUI selectedNumberText;

    [SerializeField]
    private Vector2 selectedPosition;
    [SerializeField]
    private float positionTweenTime;

    [Header("Discard")]
    [SerializeField]
    private Image discardButton;
    [SerializeField]
    private Image discardIcon;
    [SerializeField]
    private Color discardButtonColor, discardButtonColorSelected;
    [SerializeField]
    private Color discardIconColor, discardIconColorSelected;


    private RectTransform _rectTransform;

    private Vector2 _unselectedPosition;

    public event System.Action<Card> OnClick;
    public event System.Action<Card> OnDiscard;

    public CardType CardType { get; private set; }
    public bool IsSelected { get; private set; }
    public bool IsDiscard { get; private set; }

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        discardButton.color = IsDiscard ? discardButtonColorSelected : discardButtonColor;
        discardIcon.color = IsDiscard ? discardIconColorSelected : discardIconColor;

        selectedNumber.SetActive(false);
    }

    public void Setup(CardType cardType, Vector2 position)
    {
        CardType = cardType;
        outterImage.color = cardType.BackgroundColor.Value;
        innerImage.color = cardType.SecondaryColor.Value;

        _unselectedPosition = position;
        _rectTransform.anchoredPosition = position;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(this);
    }

    public void Select(int number)
    {
        if (IsSelected)
            return;
        
        if (IsDiscard)
            UnDiscard();

        selectedNumber.SetActive(true);
        selectedNumberText.text = number.ToString();

        Tween.MoveTo(_rectTransform, _unselectedPosition, _unselectedPosition + selectedPosition, positionTweenTime);

        IsSelected = true;
    }

    public void Unselect()
    {
        if (!IsSelected)
            return;

        selectedNumber.SetActive(false);

        Tween.MoveTo(_rectTransform, _rectTransform.anchoredPosition, _unselectedPosition, positionTweenTime);

        IsSelected = false;
    }

    public void UI_Discard()
    {
        IsDiscard = !IsDiscard;

        discardButton.color = IsDiscard ? discardButtonColorSelected : discardButtonColor;
        discardIcon.color = IsDiscard ? discardIconColorSelected : discardIconColor;

        OnDiscard.Invoke(this);
    }
    public void UnDiscard()
    {
        IsDiscard = false;

        discardButton.color = IsDiscard ? discardButtonColorSelected : discardButtonColor;
        discardIcon.color = IsDiscard ? discardIconColorSelected : discardIconColor;

        OnDiscard.Invoke(this);
    }
}
