using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StringList", menuName = "ScriptableObjects/StringList", order = 1)]
public class StringList : ScriptableObject
{
    public string[] List;

    [TextArea(8, 12)]
    public string InputMassiveValue;

    [ContextMenu("Massive Add")]
    public void AddValue()
    {
        var l =  new List<string> { };
        l.AddRange(List);
        l.AddRange(InputMassiveValue.Split("\n"));
        InputMassiveValue = "";

        List = l.ToArray();
    }
}
