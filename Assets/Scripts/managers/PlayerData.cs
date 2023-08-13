using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int[] GT1Pn3;
    public int[] GT2Pn3;
    public int LGT1;
    public string L;
    public int M;
    public int S;
    public int H1;
    public int H2;

    public PlayerData()
    {
        GT1Pn3 = new int[GameDesignManager.MAX_LVL_TYPE_1]; //save of level progress for type 1 of game 2/2
        GT2Pn3 = new int[GameDesignManager.MAX_LVL_TYPE_2]; //save of level progress for type 2 of game 3/3
        LGT1 = 0; //last played game type
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;
        H1 = 0;
        H2 = 0;
        Debug.Log("created PlayerData instance");
    }


}
