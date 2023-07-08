using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image outterImage;
    [SerializeField]
    private Image innerImage;
    [SerializeField]
    private TextMeshProUGUI cardName;

    [SerializeField]
    private GameObject selectedNumber;
    [SerializeField]
    private TextMeshProUGUI selectedNumberText;

    [SerializeField]
    private Vector2 selectedPosition;
    [SerializeField]
    private float positionTweenTime;
    [SerializeField]
    private float discardTime;

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
        cardName.text = cardType.CardName;

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

    public void DiscardAndDestroy()
    {
        Tween.FloatTween(
            1f,
            0f,
            discardTime,
            (float value) => canvasGroup.alpha = value);
        Tween.MoveTo(
            _rectTransform,
            _rectTransform.anchoredPosition,
            _rectTransform.anchoredPosition + Vector2.down * 20f,
            discardTime,
            () => Destroy(gameObject)
            );
    }


    public void MoveUp()
    {
        Tween.MoveTo(
            _rectTransform,
            _rectTransform.anchoredPosition,
            _rectTransform.anchoredPosition + Vector2.up * 40f,
            0.3f);
    }

    public void Use()
    {
        Tween.FloatTween(
            0f,
            1f,
            0.5f,
            (float value) => {
                canvasGroup.alpha = 1 - value;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.5f, value);
            
        }, () => Destroy(gameObject));
    }
}
