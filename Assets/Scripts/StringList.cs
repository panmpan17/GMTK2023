using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StringList", menuName = "ScriptableObjects/StringList", order = 1)]
public class StringList : ScriptableObject
{
    public string[] List;
}
