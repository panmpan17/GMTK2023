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
            GameManager.ins.DiscardCards(card.CardType);

            yield return new WaitForSeconds(0.4f);

            GameManager.ins.CurrentPlayer.ReactToCard(card.CardType);

            if (GameManager.ins.CurrentPlayer.Patient <= 0)
            {
                i++;
                for (; i < length; i++)
                {
                    cards[i].DiscardAndDestroy();
                    GameManager.ins.DiscardCards(cards[i].CardType);
                }
                bool failed = GameManager.ins.Fail();

                
                PlayerDisplay.ins.ShowSkipSign();
                yield return new WaitForSeconds(1.2f);
                PlayerDisplay.ins.Leave();
                yield return new WaitForSeconds(1f);

                if (failed)
                    yield break;

                CardChooser.ins.Redraw();

                yield break;
            }

            yield return new WaitForSeconds(0.35f);
        }

        // TODO: 

        yield return new WaitForSeconds(0.35f);

        PlayerDisplay.ins.ShowThanksSign();
        bool success = GameManager.ins.Success();

        yield return new WaitForSeconds(0.8f);
        PlayerDisplay.ins.Leave();
        yield return new WaitForSeconds(1f);

        if (!success)
            CardChooser.ins.Redraw();
    }

    // IEnumerator PlayerLeave()
    // {
    //     PlayerDisplay.ins.ShowSkipSign();
    //     yield return new WaitForSeconds(0.6f);
    //     PlayerDisplay.ins.Leave();
    // }
}
