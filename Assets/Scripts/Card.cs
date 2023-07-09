using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private TextMeshProUGUI[] cardNames;
    [SerializeField]
    private Image typeImage;

    [SerializeField]
    private Vector2 selectedPosition;
    [SerializeField]
    private float positionTweenTime;
    [SerializeField]
    private AnimationCurve positionTweenCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Select Index")]
    [SerializeField]
    private Image selectedNumber;
    [SerializeField]
    private Sprite[] selectedNumberSprites;

    [Header("Discard")]
    [SerializeField]
    private float discardTime;
    [SerializeField]
    private Image discardButton;
    [SerializeField]
    private Image discardIcon;
    [SerializeField]
    private Color discardButtonColor, discardButtonColorSelected;
    [SerializeField]
    private Color discardIconColor, discardIconColorSelected;

    [Header("Sound")]
    [SerializeField]
    private UISoundEffect selectSound;
    [SerializeField]
    private UISoundEffect unselectSound, hoverSound;
    [SerializeField]
    private UISoundEffect cardSendSound, useSound;


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

        selectedNumber.enabled = false;
    }

    public void Setup(CardType cardType, Vector2 position, float rotationAngle)
    {
        CardType = cardType;
        // outterImage.color = cardType.BackgroundColor.Value;
        // innerImage.color = cardType.SecondaryColor.Value;

        foreach (var cardName in cardNames)
            cardName.text = cardType.CardName;

        typeImage.sprite = cardType.CardTypeIcon;
        typeImage.rectTransform.sizeDelta = cardType.CardTypeIcon.rect.size;

        _unselectedPosition = position;
        // _rectTransform.anchoredPosition = position;

        Tween.MoveTo(_rectTransform, _rectTransform.anchoredPosition, position, 0.6f, positionTweenCurve);
        Tween.FloatTween(0, rotationAngle, 0.3f, (value) => _rectTransform.rotation = Quaternion.Euler(0, 0, value), positionTweenCurve);

        cardSendSound.Play();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(this);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (IsSelected)
            return;

        Vector2 delta = transform.rotation * (Vector3)selectedPosition;
        Tween.MoveTo(
            _rectTransform,
            _rectTransform.anchoredPosition,
            _unselectedPosition + delta, positionTweenTime);
        
        hoverSound.Play();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (IsSelected)
            return;

        Tween.MoveTo(_rectTransform, _rectTransform.anchoredPosition, _unselectedPosition, positionTweenTime);
    }

    public void Select(int index)
    {
        if (IsSelected)
        {
            selectedNumber.sprite = selectedNumberSprites[index];
            return;
        }
        
        if (IsDiscard)
            UnDiscard();

        selectedNumber.sprite = selectedNumberSprites[index];
        selectedNumber.enabled = true;


        Vector2 delta = transform.rotation * (Vector3)(selectedPosition * 1.5f);
        Tween.MoveTo(
            _rectTransform,
            _rectTransform.anchoredPosition,
            _unselectedPosition + delta, positionTweenTime);

        IsSelected = true;

        selectSound.Play();
    }

    public void Unselect()
    {
        if (!IsSelected)
            return;

        selectedNumber.enabled = false;

        Tween.MoveTo(_rectTransform, _rectTransform.anchoredPosition, _unselectedPosition, positionTweenTime);

        IsSelected = false;

        unselectSound.Play();
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
            positionTweenCurve,
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

        useSound.Play();
    }
}
