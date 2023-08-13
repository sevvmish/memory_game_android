using UnityEngine;
using System.Linq;

public class SaveLoadManager
{
    //[DllImport("__Internal")]
    //private static extern void SaveExtern(string data);

    //[DllImport("__Internal")]
    //private static extern void LoadExtern();

    private const string ID = "Playerdata1_01";

    public static void Save()
    {
        switch(Globals.GameType)
        {
            case 1:
                if (Globals.GameLevel < Globals.MainPlayerData.GT1Pn3.Length) Globals.MainPlayerData.GT1Pn3[Globals.GameLevel] = 1;
                Globals.MainPlayerData.LGT1 = 1;
                break;

            case 2:
                if (Globals.GameLevel < Globals.MainPlayerData.GT2Pn3.Length) Globals.MainPlayerData.GT2Pn3[Globals.GameLevel] = 1;
                Globals.MainPlayerData.LGT1 = 2;
                break;
        }

        Globals.MainPlayerData.L = Globals.CurrentLanguage;
        Globals.MainPlayerData.M = Globals.IsMobilePlatform ? 1 : 0;
        Globals.MainPlayerData.S = Globals.IsSoundOn ? 1 : 0;

        string data = JsonUtility.ToJson(Globals.MainPlayerData);
        //Debug.Log("saved: " + data);
        PlayerPrefs.SetString(ID, data);
    }

    public static void Load()
    {
        string fromSave = "";

        fromSave = PlayerPrefs.GetString(ID);

        if (string.IsNullOrEmpty(fromSave))
        {
            Globals.MainPlayerData = new PlayerData();
        }
        else
        {
            Globals.MainPlayerData = JsonUtility.FromJson<PlayerData>(fromSave);
        }
    }

}
