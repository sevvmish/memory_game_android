using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickOnCurrentLevel : MonoBehaviour
{
    public int LevelType;
    public int CurrentLevel;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => 
        {
            if (CurrentLevel == 0) return;

            switch(LevelType)
            {
                case 1:
                    MainMenu.RepeteGame(LevelType, CurrentLevel);
                    break;

                case 2:
                    MainMenu.RepeteGame(LevelType, CurrentLevel);
                    break;
            }
            
        });
    }
}
