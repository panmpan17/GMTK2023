using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using MPack;


public class PlayerDisplay : MonoBehaviour
{
    public static PlayerDisplay ins;

    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private Vector2 slideInPosition;
    [SerializeField]
    private Vector2 slideOutPosition;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI passionText;
    [SerializeField]
    private FillBarControl patientBar;
    [SerializeField]
    private GameObject skipSign, thanksSign;
    [SerializeField]
    private string passionFormat = "Passion: {0}";

    [SerializeField]
    private Image[] tagIcons;

    [SerializeField]
    private float slideInTime = 0.5f;

    public UnityEvent OnHide;

    private Color _transparent = new Color(1, 1, 1, 0);

    void Awake()
    {
        ins = this;
        playerName.color = _transparent;
        skipSign.SetActive(false);
        thanksSign.SetActive(false);
    }

    
    public void Display(PlayerScenario.Player player)
    {
        playerImage.sprite = player.Avatar;
        playerImage.enabled = true;
        playerName.text = player.Name;
        passionText.text = string.Format(passionFormat, player.Patient);

        patientBar.SetFillAmount(player.Patient / 7f);

        Tween.ColorTween(_transparent, Color.white, slideInTime, (value) => { playerName.color = value; playerImage.color = value; });
        Tween.MoveTo(playerImage.rectTransform, slideOutPosition, slideInPosition, slideInTime);

        int iconIndex = 0;
        for (int i = 0; i < player.Characteristics.Length; i++)
        {
            if (player.Characteristics[i] == null)
                continue;

            tagIcons[iconIndex].sprite = player.Characteristics[i].Icon;
            tagIcons[iconIndex].enabled = true;
            iconIndex++;
        }

        for (int i = iconIndex; i < tagIcons.Length; i++)
        {
            tagIcons[i].enabled = false;
        }
    }

    public void UpdatePassion()
    {
        passionText.text = string.Format(passionFormat, GameManager.ins.CurrentPlayer.Patient);
        patientBar.SetFillAmount(GameManager.ins.CurrentPlayer.Patient / 7f);
    }

    public void ShowSkipSign()
    {
        skipSign.SetActive(true);
    }
    public void ShowThanksSign()
    {
        thanksSign.SetActive(true);
    }

    public void Leave()
    {
        skipSign.SetActive(false);
        thanksSign.SetActive(false);
        Tween.ColorTween(Color.white, _transparent, slideInTime, (value) => { playerName.color = value; playerImage.color = value; }, () => playerName.text = "");
        Tween.MoveTo(playerImage.rectTransform, slideInPosition, slideOutPosition, slideInTime, () => playerImage.enabled = false);
        // gameObject.SetActive(false);
    }

    public void Hide()
    {
        skipSign.SetActive(false);
        thanksSign.SetActive(false);
        // playerImage.enabled = false;
        playerName.text = "";

        for (int i = 0; i < tagIcons.Length; i++)
        {
            tagIcons[i].enabled = false;
        }

        OnHide.Invoke();
        // gameObject.SetActive(false);
    }
}
