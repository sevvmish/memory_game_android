using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "Languages", order = 1)]
public class Translation : ScriptableObject
{
    //HEROES==============================================
    public string playText;
    public string loadingText;
    public string next;
    public string DeleteProgress;

    [Header("Types")]
    public string Type1Name;
    public string Type2Name;
    public string Type3Name;

    public string Type1Description;
    public string Type2Description;
    public string Type3Description;

    public string winText;
    public string loseText;

    public string getMoreSecondsInfo;
    public string secondsAmountPart;
    public string forRewarded;

    public string hint1;
    public string hint2;

    public Translation() { }
}
