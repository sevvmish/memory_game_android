using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "pack", menuName = "ScriptableObjects/my packs")]
public class Pack : ScriptableObject
{
    public int ID;
    public string PackName;
    public int PackDifficulty;
    public Sprite[] sprites;
}
