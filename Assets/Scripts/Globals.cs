using System;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static GameDesignManager GameDesignManager;
    public static PlayerData MainPlayerData;
    public static int GameType;// = 1;
    public static int GameLevel;// = 0;
    public static int Difficulty;// = 0;
    public static int RewardedSeconds = 20;// = 10;
    public static int AdditionalPanelsAmount = 0;

    //GAME PRESETS
    public const float BASE_VOLUME = 0.8f;
    public static bool IsMobilePlatform;
    public static bool IsInitiated;
    public static bool IsSoundOn = true;

    public static bool IsRepeteGame;
    public static int GameTypeRepete = 1;// = 1;
    public static int GameLevelRepete = 3;// = 0;

    public static int HowManyLost = 0;

    public static DateTime TimeWhenStartedPlaying;

    public const float INTERVAL_FOR_INTERSTITITAL = 30;
    public const float INTERVAL_FOR_REWARDED = 65;
    public static DateTime TimeWhenLastInterstitialWas;
    public static int InterstitialsAmount;
    public static DateTime TimeWhenLastRewardedWas;
    public static int RewardedAmount;

    public static string CurrentLanguage;
    public static int PreviousPackIDNumber;

    public static float PanelSimpleRotationSpeed = 0.3f;// = 0.35f;
    public static PairGroupTypes CurrentPairGroupType = PairGroupTypes.two;


    //level customization============================
    public static float StageDurationInSec = 100;
    public static Vector2 PanelsNumber = new Vector2(3, 2);

    //===============================================

    public static float PanelTimeForShowing = 1.4f;

    /*
    {
        get
        {
            switch(CurrentPairGroupType)
            {
                case PairGroupTypes.two:
                    return 1.1f;
                case PairGroupTypes.three:
                    return 1.3f;
            }

            return 1.1f;
        }
    }
    */

    public const string INTERSTITIAL_ID = "R-M-2588274-3";
    public const string REWARDED_ID = "R-M-2588274-2";
}


