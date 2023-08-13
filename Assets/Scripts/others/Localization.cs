using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization
{
    private Translation translation;
    private Localization(string lang) 
    {
        switch(lang)
        {
            case "Russian":
                translation = Resources.Load<Translation>("languages/russian");                                
                break;

            case "Belarusian":
                translation = Resources.Load<Translation>("languages/russian");
                break;

            case "Ukrainian":
                translation = Resources.Load<Translation>("languages/russian");
                break;

            case "English":
                translation = Resources.Load<Translation>("languages/english");
                break;

            default:
                //translation = Resources.Load<Translation>("languages/russian");
                translation = Resources.Load<Translation>("languages/english");
                break;
        }
        
    }

    private static Localization instance;
    public static Localization GetInstanse(string lang)
    {
        if (instance == null)
        {
            instance = new Localization(lang);
        }

        return instance;
    }

    public Translation GetCurrentTranslation() => translation;

}
