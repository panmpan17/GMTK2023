using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TellStoryDisplay : MonoBehaviour
{
    public static TellStoryDisplay ins;

    [SerializeField]
    private float waitTime;


    void Awake()
    {
        ins = this;
    }

    public void StartTelling(Card[] cards, bool hasDiscard)
    {
        StartCoroutine(Tell(cards, hasDiscard));
    }

    IEnumerator Tell(Card[] cards, bool hasDiscard)
    {
        if (hasDiscard)
        {
            yield return new WaitForSeconds(waitTime * 2);
        }

        int length = cards.Length;
        for (int i = 0; i < length; i++)
        {
            var card = cards[i];
            card.MoveUp();
            yield return new WaitForSeconds(0.35f);
            card.Use();
            yield return new WaitForSeconds(0.4f);

            GameManager.ins.CurrentPlayer.ReactToCard(card.CardType);

            if (GameManager.ins.CurrentPlayer.Passion <= 0)
            {
                yield return StartCoroutine(PlayerLeave());

                i++;
                for (; i < length; i++)
                    Destroy(cards[i].gameObject);

                CardChooser.ins.Redraw();
                yield break;
            }

            yield return new WaitForSeconds(0.35f);
        }

        // TODO: 

        yield return new WaitForSeconds(0.35f);

        CardChooser.ins.Redraw();
    }

    IEnumerator PlayerLeave()
    {
        PlayerDisplay.ins.SkipAndLeave();
        yield return new WaitForSeconds(.5f);
    }
}
