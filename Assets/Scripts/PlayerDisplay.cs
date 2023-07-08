using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerDisplay : MonoBehaviour
{
    public static PlayerDisplay ins;

    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI passionText;
    [SerializeField]
    private GameObject skipSign;
    [SerializeField]
    private string passionFormat = "Passion: {0}";

    void Awake()
    {
        ins = this;
    }

    
    public void Display(PlayerScenario.Player player)
    {
        playerImage.sprite = player.Avatar;
        playerName.text = player.Name;
        passionText.text = string.Format(passionFormat, player.Passion);
        gameObject.SetActive(true);
    }

    public void UpdatePassion()
    {
        passionText.text = string.Format(passionFormat, GameManager.ins.CurrentPlayer.Passion);
    }

    public void SkipAndLeave()
    {
        gameObject.SetActive(false);
    }
}
