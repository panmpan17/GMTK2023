using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MPack;

public class HUDDisplay : MonoBehaviour
{
    public static HUDDisplay ins;

    [SerializeField]
    private TextMeshProUGUI unusedCardsCountText;
    [SerializeField]
    private TextMeshProUGUI discardedCardsCountText;
    [SerializeField]
    private TextMeshProUGUI successCountText;
    [SerializeField]
    private IntReference maxSuccessCount;
    [SerializeField]
    private TextMeshProUGUI failCountText;
    [SerializeField]
    private IntReference maxFailCount;
    [SerializeField]
    private string failCountFormat = "{0}/{1}";

    void Awake()
    {
        ins = this;
    }

    public void UpdateCardsCount(int unusedCount, int discardedCount)
    {
        unusedCardsCountText.text = unusedCount.ToString();
        discardedCardsCountText.text = discardedCount.ToString();
    }

    public void UpdateSuccessFailCount(int successCount, int failCount)
    {
        successCountText.text = string.Format(failCountFormat, successCount, maxSuccessCount.Value);
        failCountText.text = string.Format(failCountFormat, failCount, maxFailCount.Value);
    }
}
