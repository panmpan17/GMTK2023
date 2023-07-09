using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPack;


[CreateAssetMenu(fileName = "PlayerScenario", menuName = "ScriptableObjects/PlayerScenario", order = 1)]
public class PlayerScenario : ScriptableObject
{
    public StringList PlayerName;
    public SpriteList Avatar;

    public RangeReference PassionRange;

    public PlayerCharacteristic.TagReact[] ProfessionReacts;
    public CharacteristicSet[] CharacteristicSets;


    public string GetRandomName() => PlayerName.GetRandom();
    public Sprite GetRandomAvatar() => Avatar.GetRandom();

    public PlayerCharacteristic[] GetRandomCharacteristics()
    {
        var characteristics = new PlayerCharacteristic[CharacteristicSets.Length];

        for (int i = 0; i < CharacteristicSets.Length; i++)
        {
            characteristics[i] = CharacteristicSets[i].GetRandom();
        }

        return characteristics;
    }

    public Player GetRandomPlayer()
    {
        var name = GetRandomName();
        var avatar = GetRandomAvatar();
        var characteristics = GetRandomCharacteristics();

        return new Player(name, PassionRange.PickRandomNumber(), avatar, ProfessionReacts, characteristics);
    }


    [System.Serializable]
    public struct CharacteristicSet
    {
        public PlayerCharacteristic[] Characteristics;

        public PlayerCharacteristic GetRandom() => Characteristics[Random.Range(0, Characteristics.Length)];
    }

    public class Player
    {
        public string Name;
        public float Patient;
        public Sprite Avatar;
        public PlayerCharacteristic.TagReact[] ProfessionReacts;
        public PlayerCharacteristic[] Characteristics;

        public Player(string name, float passion, Sprite avatar, PlayerCharacteristic.TagReact[] professionReacts, PlayerCharacteristic[] characteristics)
        {
            Name = name;
            Avatar = avatar;

            Patient = Mathf.Round(passion * 10f) / 10f;;

            ProfessionReacts = professionReacts;
            Characteristics = characteristics;
        }

        public float ReactToCard(CardType cardType)
        {
            float passionChange = -0.3f;

            foreach (var react in ProfessionReacts)
            {
                if (Contains(cardType, react.Tag))
                {
                    passionChange += react.Value;
                }
            }

            foreach (var characteristic in Characteristics)
            {
                if (characteristic == null)
                    continue;
                foreach (var react in characteristic.TagReacts)
                {
                    if (Contains(cardType, react.Tag))
                    {
                        passionChange += react.Value;
                    }
                }
            }

            Patient += passionChange;
            PlayerDisplay.ins.UpdatePassion();
            return passionChange;
        }

        bool Contains(CardType cardType, CardTag _tag)
        {
            foreach (var tag in cardType.Tags)
            {
                if (tag == _tag)
                    return true;
            }

            return false;
        }
    }
}


// public enum Profession
// {
//     Tank,
//     Healer,
//     DamageArcher,
//     DamageSwordsman,
// }