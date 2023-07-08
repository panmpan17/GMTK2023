using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "CardDecks", menuName = "ScriptableObjects/CardDecks", order = 1)]
public class CardDecks : ScriptableObject
{
    public CardAmount[] Cards;

    public CardType[] CardInputs;

    public int TotalCount {
        get {
            int count = 0;
            foreach (var card in Cards)
            {
                count += card.Amount;
            }
            return count;
        }
    }

    public CardType[] TotalCards {
        get {
            var cards = new List<CardType>();
            foreach (var card in Cards)
            {
                for (int i = 0; i < card.Amount; i++)
                {
                    cards.Add(card.CardType);
                }
            }
            return cards.ToArray();
        }
    }


    void OnValidate()
    {
        if (CardInputs == null || CardInputs.Length <= 0)
            return;

        var cardAmounts = new List<CardAmount>();
        cardAmounts.AddRange(Cards);

        foreach (var card in CardInputs)
        {
            var cardAmount = new CardAmount();
            cardAmount.CardType = card;
            cardAmount.Amount = 1;
            cardAmounts.Add(cardAmount);
        }

        Cards = cardAmounts.ToArray();
        CardInputs = new CardType[0];
    }

    
    [System.Serializable]
    public struct CardAmount
    {
        public CardType CardType;
        public int Amount;

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(CardAmount))]
        public class CardAmountDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                EditorGUI.BeginProperty(position, label, property);

                var cardType = property.FindPropertyRelative("CardType");
                var amount = property.FindPropertyRelative("Amount");

                var cardTypeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                var amountRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);

                EditorGUI.PropertyField(cardTypeRect, cardType);
                EditorGUI.PropertyField(amountRect, amount);

                EditorGUI.EndProperty();
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUIUtility.singleLineHeight * 2;
            }
        }
#endif
    }
}
