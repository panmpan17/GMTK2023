using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDDisplay : MonoBehaviour
{
    public static HUDDisplay ins;

    [SerializeField]
    private TextMeshProUGUI unusedCardsCountText;
    [SerializeField]
    private TextMeshProUGUI discardedCardsCountText;

    void Awake()
    {
        ins = this;
    }

    public void UpdateCardsCount(int unusedCount, int discardedCount)
    {
        unusedCardsCountText.text = unusedCount.ToString();
        discardedCardsCountText.text = discardedCount.ToString();
    }
}
